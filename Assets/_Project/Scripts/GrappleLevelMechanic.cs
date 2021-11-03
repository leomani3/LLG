using System;
using System.Collections;
using DG.Tweening;
using MyBox;
using UnityEngine;

public class GrappleLevelMechanic : MonoBehaviour, ILevelMechanic
{
    public LayerMask grappableLayers;
    public float grappleMinLength;
    public float grapplePullingSpeed;
    public LayerMask layerMask;
    public float lineWidth;
    public Material lineMaterial;

    private bool _connected;
    private Vector3 _worldMousePos;
    private Camera _mainCam;
    private RaycastHit2D[] _raycastHits;
    private GameObject _objectConnectedTo;
    private Tween _moveTween;
    private LineRenderer _lineRenderer;
    private Rigidbody2D _rigidbody;
    private PlayerController _playerController;

    private void Awake()
    {
        _mainCam = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if (_connected && _lineRenderer != null)
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _objectConnectedTo.transform.position);
        }
    }

    private IEnumerator Pull()
    {
        _playerController.SetGravity(false);
        while (Vector3.Distance(transform.position, _raycastHits[0].point) > grappleMinLength)
        {
            _rigidbody.AddForce((_raycastHits[0].point - transform.position.ToVector2()) * 10, ForceMode2D.Force);
            yield return null;
        }
        
        _playerController.Rb.velocity = Vector2.zero;
        
        HingeJoint2D hingeJoint = _objectConnectedTo.GetComponent<HingeJoint2D>();
        if (hingeJoint == null)
            hingeJoint = _raycastHits[0].collider.gameObject.AddComponent<HingeJoint2D>();

        hingeJoint.connectedBody = GetComponent<Rigidbody2D>();
        hingeJoint.enableCollision = true;

        hingeJoint.anchor = _objectConnectedTo.transform.InverseTransformPoint(_raycastHits[0].point);

        _playerController.SetGravity(true);
    }

    public void Interact()
    {
        if (!_connected)
        {
            _worldMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition.SetZ(Mathf.Abs(_mainCam.transform.position.z)));
            Ray ray = new Ray(transform.position, _worldMousePos - transform.position);

            _raycastHits = Physics2D.RaycastAll(ray.origin, ray.direction, 100, layerMask);
            if (_raycastHits.Length > 0 && _raycastHits[0].collider.gameObject == gameObject)
                _raycastHits = _raycastHits.RemoveAt(0);


            if (_raycastHits.Length > 0 && _raycastHits[0].collider != null)
            {
                _connected = true;

                if (_lineRenderer == null)
                    _lineRenderer = gameObject.AddComponent<LineRenderer>();
                _lineRenderer.positionCount = 2;
                _lineRenderer.widthMultiplier = 0.1f;
                _lineRenderer.material = lineMaterial;

                _objectConnectedTo = _raycastHits[0].collider.gameObject;

                StartCoroutine(Pull());
            }
        }
        else
        {
            Disconnect();
        }
    }

    private void Disconnect()
    {
        if (_lineRenderer != null)
            Destroy(_lineRenderer);
        
        
        StopAllCoroutines();
        _playerController.SetGravity(true);  
        
        
        _moveTween.Kill();
        Destroy(_objectConnectedTo.GetComponent<HingeJoint2D>());
        _connected = false;
    }
}