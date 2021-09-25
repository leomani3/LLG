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
        
        if (Input.GetKey(KeyCode.D))
            _moveVector += Vector2.right * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
            _moveVector += Vector2.left * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && _grounded)
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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