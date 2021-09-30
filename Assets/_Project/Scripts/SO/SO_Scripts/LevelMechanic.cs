using UnityEngine;

public abstract class LevelMechanic : ScriptableObject
{
    public GameObject assignedObject;
    public abstract void Interact();
}