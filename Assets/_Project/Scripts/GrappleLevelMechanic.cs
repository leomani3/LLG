using System;
using MyBox;
using UnityEngine;

public class GrappleLevelMechanic : MonoBehaviour, ILevelMechanic
{
    public LayerMask grappableLayers;
    public float grappleMinLength;
    public float grapplePullingSpeed;

    private bool _connected;
    private Vector3 _worldMousePos;
    private Camera _mainCam;

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
        }
        else
        {
            Disconnect();
        }
    }

    private void Disconnect()
    {
        
    }
}