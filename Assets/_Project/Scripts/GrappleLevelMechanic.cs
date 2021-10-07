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

    private bool _connected;
    private Vector3 _worldMousePos;
    private Camera _mainCam;
    private RaycastHit2D raycastHit;
    private GameObject objectConnectedTo;
    private Tween moveTween;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    public void Interact()
    {
        if (!_connected)
        {
            _worldMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition.SetZ(Mathf.Abs(_mainCam.transform.position.z)));
            Ray ray = new Ray(transform.position, _worldMousePos - transform.position);
            Debug.DrawRay(ray.origin, ray.direction * 100);

            raycastHit = Physics2D.Raycast(ray.origin, ray.direction, 100, layerMask);
            if (raycastHit != null)
            {
                _connected = true;
                
                objectConnectedTo = raycastHit.collider.gameObject;
                //créer hinge joint sur l'objet touché (il doit avoir un rigidbody)
                HingeJoint2D hingeJoint = objectConnectedTo.GetComponent<HingeJoint2D>();
                if (hingeJoint == null)
                    hingeJoint = raycastHit.collider.gameObject.AddComponent<HingeJoint2D>();
     
                hingeJoint.connectedBody = GetComponent<Rigidbody2D>();
                
                //transformer le point de contact de world pos en local pos de l'object
                hingeJoint.anchor = objectConnectedTo.transform.InverseTransformPoint(raycastHit.point + new Vector2(0.5f, 0));
                //hingeJoint.connectedAnchor = hitObject.transform.InverseTransformPoint((transform.position.ToVector2() -  hingeJoint.anchor).normalized * 1);
                // set l'anchor point du hinge joint sur cette position
                moveTween = transform.DOMove((raycastHit.point), 0.5f);
            }
        }
        else
        {
            Disconnect();
        }
    }

    private void Disconnect()
    {
        moveTween.Kill();
        Destroy(objectConnectedTo.GetComponent<HingeJoint2D>());
        _connected = false;
    }
}