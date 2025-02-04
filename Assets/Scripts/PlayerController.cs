using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Coyote Time Settings")]
    public float coyoteTime = 0.2f; // Duración del coyote time
    private float coyoteTimeCounter;
    private bool JumpDone;

    [Header("Input Buffer Settings")]
    public float jumpBufferTime = 0.2f; // Duración del input buffer
    private float jumpBufferCounter;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator anim;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Comprobar si el personaje está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Manejo del coyote time
        if (isGrounded)
        {
            JumpDone = false;
            anim.SetBool("Grounded", true);
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            anim.SetBool("Grounded", false);
            JumpDone = true;
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Manejo del input buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Movimiento horizontal
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Verificar si puede saltar
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !JumpDone)
        {
            Jump();
            jumpBufferCounter = 0f; // Reiniciar el buffer después de saltar
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}