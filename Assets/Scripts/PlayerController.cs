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
    [SerializeField] private KeyCode rightKey;
    [SerializeField] private KeyCode secondaryRightKey;
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode secondaryLeftKey;
    [SerializeField] private KeyCode interactKey;

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
        if (pseudo)
        {
            pseudo.text = ServerInfos.Pseudo;
        }
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
        
        if (Input.GetKey(rightKey) || Input.GetKey(secondaryRightKey))
            _moveVector += Vector2.right * moveSpeed * Time.deltaTime;

        if (Input.GetKey(leftKey) || Input.GetKey(secondaryLeftKey))
            _moveVector += Vector2.left * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && _grounded)
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
        if (Input.GetKeyDown(interactKey))
            Interact();
    }

    public void Interact()
    {
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