
using UnityEngine;

[CreateAssetMenu(fileName = "LevelMechanicData", menuName = "ScriptableObjects/LevelMechanicData", order = 1)]
public class LevelMechanicData : ScriptableObject
{
    public GameobjectPoolRef bumpFXPoolRef;
    public LayerMask bumpLayers;
    public float bumpRadius;
    public float bumpForce;

    public LayerMask grappleLayerMask;
    public float grappleMinLength;
    public float grappleLineWidth;
}