using System;
using MLAPI;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckPositionOffset;
    [SerializeField] private float gravityMulitplier;
    [SerializeField] private PlayerKeyBinding playerKingBinding;
    [SerializeField] private GameobjectPoolRef bumpFXPoolRef;

    [Separator("Bump")]
    [SerializeField] private float ejectionForce;
    [SerializeField] private float bumpRadius;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D _rb;
    private Vector2 _moveVector;
    private bool _grounded;

    // Je sais pas si y'a une meilleure méthode que faire ça, sorry
    public Text pseudo;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Dans le futur, le nom sera attribué par le manager de la scène
        // pour chaque player.
        //if (pseudo)
        //{
        //    pseudo.text = ServerInfos.Pseudo;
        //}
    }

    private void Update()
    {
        GroundCheck();

        HandleMovement();
    }

    private void FixedUpdate()
    {
        _rb.AddForce(new Vector2(_moveVector.x, 0), ForceMode2D.Force);
        _rb.velocity += new Vector2(0, Physics.gravity.y * gravityMulitplier);
    }

    private void HandleMovement()
    {
        _moveVector = Vector2.zero;

        if (Input.GetKey(playerKingBinding.right))
            _moveVector += Vector2.right * moveSpeed * Time.deltaTime;

        if (Input.GetKey(playerKingBinding.left))
            _moveVector += Vector2.left * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(playerKingBinding.jump) && _grounded)
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (Input.GetKeyDown(playerKingBinding.interact))
            Interact();
    }

    public void Interact()
    {
        GameObject spawnedFX = bumpFXPoolRef.gameObjectPool.Spawn(transform.position, Quaternion.identity, transform);
        spawnedFX.transform.localScale = Vector3.one * (bumpRadius * 2);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bumpRadius, playerLayer);
        foreach (Collider2D collider in colliders)
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Eject(player.transform.position - transform.position, ejectionForce);
            }
        }
    }

    public void Eject(Vector2 direction, float force)
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapCircle(transform.position.ToVector2() + groundCheckPositionOffset, groundCheckRadius, groundLayer) != null)
            _grounded = true;
        else
            _grounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position.ToVector2() + groundCheckPositionOffset, groundCheckRadius);
    }
}
