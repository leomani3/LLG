using System;
using MLAPI;
using MyBox;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckPositionOffset;
    [SerializeField] private float gravityMulitplier;
    [SerializeField] private PlayerKeyBinding playerKeyBinding;
    [SerializeField] private GameobjectPoolRef bumpFXPoolRef;

    [Separator("Bump")]
    [SerializeField] private float ejectionForce;
    [SerializeField] private float bumpRadius;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D _rb;
    private Vector2 _moveVector;
    private bool _grounded;
    private LobbyPlayerState _lobbyPlayerState;
    private ILevelMechanic _levelMechanic;

    [Header("References")]
    [SerializeField] private TMP_Text pseudoText;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GroundCheck();

        HandleMovement();
    }

    public void Interact()
    {

    }

    private void FixedUpdate()
    {
        _rb.AddForce(new Vector2(_moveVector.x, 0), ForceMode2D.Force);
        _rb.velocity += new Vector2(0, Physics.gravity.y * gravityMulitplier);
    }

    private void HandleMovement()
    {
        _moveVector = Vector2.zero;

        if (Input.GetKey(playerKeyBinding.right))
            _moveVector += Vector2.right * moveSpeed * Time.deltaTime;

        if (Input.GetKey(playerKeyBinding.left))
            _moveVector += Vector2.left * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(playerKeyBinding.jump) && _grounded)
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (Input.GetKeyDown(playerKeyBinding.interact))
            _levelMechanic.Interact();
    }

    public void Eject(Vector2 direction, float force)
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void SetLobbyPlayerState(LobbyPlayerState lps)
    {
        _lobbyPlayerState = lps;
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        pseudoText.text = _lobbyPlayerState.PlayerName;
        GetComponent<Renderer>().material.color = GetColorFromNumber(_lobbyPlayerState.NumberColor);
    }

    public void SetLevelMechanic(ILevelMechanic levelMechanic)
    {
        _levelMechanic = levelMechanic;
    }

    public void SetName(string n)
    {
        pseudoText.text = n;
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

    // A déplacer dans les settings et à mettre en static
    private Color GetColorFromNumber(int i)
    {
        switch(i)
        {
            default:
            case 0:
                return Color.black;
            case 1:
                return Color.blue;
            case 2:
                return Color.cyan;
            case 3:
                return Color.green;
            case 4:
                return Color.magenta;
            case 5:
                return Color.red;
            case 6:
                return Color.white;
            case 7:
                return Color.yellow;
        }
    }
}
