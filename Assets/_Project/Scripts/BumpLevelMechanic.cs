using UnityEngine;

public class BumpLevelMechanic : MonoBehaviour, ILevelMechanic
{
    public GameobjectPoolRef bumpFXPoolRef;
    public LayerMask bumpLayers;
    public float bumpRadius;
    public float bumpForce;

    public void Interact()
    {
        GameObject spawnedFX = bumpFXPoolRef.gameObjectPool.Spawn(transform.position, Quaternion.identity, transform);
        spawnedFX.transform.localScale = Vector3.one * (bumpRadius * 2);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bumpRadius, bumpLayers);
        foreach (Collider2D collider in colliders)
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Eject(player.transform.position - transform.position, bumpForce);
            }
        }
    }
}