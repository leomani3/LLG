using UnityEngine;

public class GrappleLevelMechanic : MonoBehaviour, ILevelMechanic
{
    public LayerMask grappableLayers;
    public float grappleMinLength;
    public float grapplePullingSpeed;

    private bool _connected;
    
    public void Interact()
    {
        if (!_connected)
        {
            Ray ray = new Ray(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.DrawRay(ray.origin, ray.direction);
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