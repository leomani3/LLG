using Lean.Pool;
using UnityEngine;

[CreateAssetMenu(fileName = "GameobjectPoolRef", menuName = "ScriptableObjects/GameobjectPoolRef", order = 1)]
public class GameobjectPoolRef : ScriptableObject
{
    public LeanGameObjectPool gameObjectPool;
}