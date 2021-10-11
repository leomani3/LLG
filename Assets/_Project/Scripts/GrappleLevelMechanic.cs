using System;
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

    private bool _connected;
    private Vector3 _worldMousePos;
    private Camera _mainCam;
    private RaycastHit2D _raycastHit;
    private GameObject _objectConnectedTo;
    private Tween _moveTween;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        if (_connected)
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _raycastHit.point);
        }
    }

    public void Interact()
    {
        if (!_connected)
        {
            _worldMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition.SetZ(Mathf.Abs(_mainCam.transform.position.z)));
            Ray ray = new Ray(transform.position, _worldMousePos - transform.position);

            _raycastHit = Physics2D.Raycast(ray.origin, ray.direction, 100, layerMask);
            if (_raycastHit.collider != null)
            {
                _connected = true;

                if (_lineRenderer == null)
                    _lineRenderer = gameObject.AddComponent<LineRenderer>();
                _lineRenderer.positionCount = 2;
                _lineRenderer.widthMultiplier = 0.1f;
                
                _objectConnectedTo = _raycastHit.collider.gameObject;
                HingeJoint2D hingeJoint = _objectConnectedTo.GetComponent<HingeJoint2D>();
                if (hingeJoint == null)
                    hingeJoint = _raycastHit.collider.gameObject.AddComponent<HingeJoint2D>();
     
                hingeJoint.connectedBody = GetComponent<Rigidbody2D>();

                hingeJoint.anchor = _objectConnectedTo.transform.InverseTransformPoint(_raycastHit.point);
                _moveTween = transform.DOMove((_raycastHit.point + (transform.position.ToVector2() - _raycastHit.point).normalized * grappleMinLength), 0.5f);
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
        
        _moveTween.Kill();
        Destroy(_objectConnectedTo.GetComponent<HingeJoint2D>());
        _connected = false;
    }
}