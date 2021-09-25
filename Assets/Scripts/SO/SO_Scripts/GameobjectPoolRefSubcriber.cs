using Lean.Pool;
using UnityEngine;

public class GameobjectPoolRefSubcriber : MonoBehaviour
{
    [SerializeField] private GameobjectPoolRef poolRef;

    private void Awake()
    {
        poolRef.gameObjectPool = GetComponent<LeanGameObjectPool>();
    }
}