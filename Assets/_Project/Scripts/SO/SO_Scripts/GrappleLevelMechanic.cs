using UnityEngine;

[CreateAssetMenu(fileName = "GrappleLevelMechanic", menuName = "ScriptableObjects/GrappleLevelMechanic", order = 1)]
public class GrappleLevelMechanic : LevelMechanic
{
    public override void Interact()
    {
        Debug.Log("grapple");
    }
}