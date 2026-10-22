using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    [Header("Movement Settings")]
    public float speed;
    public float jumpForce;
    public float fallGravity;

    [Header("Double Jump Settings")]
    public bool shouldDoubleJump;
    public bool canDoubleJump;
    public float doubleJumpForce;

    [Header("Grounded")]
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public Transform groundCheck; 
    public bool isGrounded;
    bool isFalling;
    float move;
    float originalGravityScale;
    Rigidbody2D rb;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += Movement;
        inputActions.Player.Jump.performed += Jump;
        inputActions.Player.Move.canceled += Movement;
        inputActions.Player.Jump.canceled += Jump;
    }
    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Move.performed -= Movement;
        inputActions.Player.Jump.performed -= Jump;
        inputActions.Player.Move.canceled -= Movement;
        inputActions.Player.Jump.canceled -= Jump;
    }
    void Movement(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>().x;
    }
    void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            else if (shouldDoubleJump)
            {
                if (canDoubleJump && context.performed && !isGrounded)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                    rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
                    canDoubleJump = false;
                }
            }
        }    
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if(isFalling)
        {
            rb.gravityScale = fallGravity;
        }else
        {
            rb.gravityScale = originalGravityScale;
        }
        if (isGrounded)
        {
            canDoubleJump = true;
            isFalling = false;
        }
        else
        {
            isFalling = rb.linearVelocity.y < -0.1f;
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocityX = move * speed;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
