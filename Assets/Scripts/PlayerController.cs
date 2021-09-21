using System;
using MLAPI;
using MyBox;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckPositionOffset;

    private Rigidbody2D _rb;
    private Vector2 _moveVector;
    private bool _grounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GroundCheck();

        HandleMovement();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_moveVector.x, _rb.velocity.y + _moveVector.y);
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