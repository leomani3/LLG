using UnityEngine;

public class BumpLevelMechanic : MonoBehaviour, ILevelMechanic
{
    public GameobjectPoolRef bumpFXPoolRef;
    public LayerMask bumpLayers;
    public float bumpRadius;
    public float bumpForce;

    private GameObject _assigneObject;

    public GameObject AssigneObject
    {
        get => _assigneObject;
        set => _assigneObject = value;
    }

    public void Interact()
    {
        GameObject spawnedFX = bumpFXPoolRef.gameObjectPool.Spawn(_assigneObject.transform.position, Quaternion.identity, _assigneObject.transform);
        spawnedFX.transform.localScale = Vector3.one * (bumpRadius * 2);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_assigneObject.transform.position, bumpRadius, bumpLayers);
        foreach (Collider2D collider in colliders)
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Eject(player.transform.position - _assigneObject.transform.position, bumpForce);
            }
        }
    }
}