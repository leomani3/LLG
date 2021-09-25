using UnityEngine;

[CreateAssetMenu(fileName = "PlayerKeyBinding", menuName = "ScriptableObjects/PlayerKeyBinding", order = 1)]
public class PlayerKeyBinding : ScriptableObject
{
    public KeyCode right;
    public KeyCode left;
    public KeyCode jump;
    public KeyCode interact;
}