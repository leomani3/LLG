using System;
using System.Collections;
using DG.Tweening;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private Grappable _objectConnectedTo;
    private Tween _moveTween;
    private LineRenderer _lineRenderer;
    private Rigidbody2D _rigidbody;
    private PlayerController _playerController;
    private HingeJoint2D _connectedJoint;

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
            _lineRenderer.SetPosition(1, _objectConnectedTo.GrappleAnchor.position);
        }
    }

    private IEnumerator Pull()
    {
        _playerController.SetGravity(false);
        while (Vector3.Distance(transform.position, _objectConnectedTo.GrappleAnchor.position) > grappleMinLength)
        {
            _rigidbody.AddForce((_objectConnectedTo.GrappleAnchor.position.ToVector2() - transform.position.ToVector2()) * 10, ForceMode2D.Force);
            yield return null;
        }
        
        _playerController.Rb.velocity = Vector2.zero;
        
        _connectedJoint = _objectConnectedTo.gameObject.AddComponent<HingeJoint2D>();

        _connectedJoint.connectedBody = GetComponent<Rigidbody2D>();
        _connectedJoint.enableCollision = true;

        _connectedJoint.anchor = _objectConnectedTo.transform.InverseTransformPoint(_objectConnectedTo.GrappleAnchor.position);

        _playerController.SetGravity(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController enemy = other.gameObject.GetComponent<PlayerController>();
        if (enemy != null && _connected)
            Disconnect();
    }

    public void Interact()
    {
        if (!_connected)
        {
            _worldMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition.SetZ(Mathf.Abs(_mainCam.transform.position.z)));
            Ray ray = new Ray(transform.position, _worldMousePos - transform.position);

            _raycastHits = Physics2D.RaycastAll(ray.origin, ray.direction, 100, layerMask);

            foreach (RaycastHit2D raycastHit2D in _raycastHits)
            {
                if (raycastHit2D.collider.GetComponent<Grappable>())
                {
                    _objectConnectedTo = raycastHit2D.collider.GetComponent<Grappable>();
                    break;
                }
            }


            if (_objectConnectedTo != null)
            {
                _connected = true;

                if (_lineRenderer == null)
                    _lineRenderer = gameObject.AddComponent<LineRenderer>();
                _lineRenderer.positionCount = 2;
                _lineRenderer.widthMultiplier = 0.1f;
                _lineRenderer.material = lineMaterial;

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
        Destroy(_connectedJoint);
        _connectedJoint = null;
        
        
        _moveTween.Kill();
        Destroy(_objectConnectedTo.GetComponent<HingeJoint2D>());
        _connected = false;
    }
}