using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappable : MonoBehaviour
{
    [SerializeField] private Transform grappleAnchor;

    public Transform GrappleAnchor
    {
        get {
            if (grappleAnchor != null)
                return grappleAnchor;
            
            return transform;
        }
    }
}