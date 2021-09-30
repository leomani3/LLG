using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BumpLevelMechanic", menuName = "ScriptableObjects/BumpLevelMechanic", order = 1)]
public class BumpLevelMechanic : LevelMechanic
{
    [SerializeField] private GameobjectPoolRef bumpFXPoolRef;
    [SerializeField] private LayerMask bumpLayers;
    [SerializeField] private float bumpRadius;
    [SerializeField] private float bumpForce;
    public override void Interact()
    {
        GameObject spawnedFX = bumpFXPoolRef.gameObjectPool.Spawn(assignedObject.transform.position, Quaternion.identity, assignedObject.transform);
        spawnedFX.transform.localScale = Vector3.one * (bumpRadius * 2);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(assignedObject.transform.position, bumpRadius, bumpLayers);
        foreach (Collider2D collider in colliders)
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Eject(player.transform.position - assignedObject.transform.position, bumpForce);
            }
        }
    }
}