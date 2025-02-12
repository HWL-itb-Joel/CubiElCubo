using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public bool conoCollected;

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

    [Header("Dead")]
    public bool isDead = false;
    public float upwardForce = 5f;
    public float fallSpeed = 5f;



    private void Start()
    {
        isDead = false;
        conoCollected = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (!isDead)
        {
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

            if (conoCollected && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.M)))
            {
                GameManager.gameManager.ChangeColor();
            }

            // Movimiento horizontal
            

            if (moveInput != 0)
            {
                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
            }

            

            // Verificar si puede saltar
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !JumpDone)
            {
                Jump();
                jumpBufferCounter = 0f; // Reiniciar el buffer después de saltar
            }
        }
        else
        {
            moveInput = 0;
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cono"))
        {
            conoCollected = true;
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("pinchos") || collision.CompareTag("Muerte"))
        {
            StartCoroutine(DeadAnim());
        }
    }

    IEnumerator DeadAnim()
    {
        isDead = true;
        rb.simulated = false;
        anim.SetTrigger("dead");

        rb.velocity = Vector2.zero;
        GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSecondsRealtime(0.5f);

        rb.simulated = true;
        rb.velocity = new Vector2(rb.velocity.x, upwardForce);

        yield return new WaitForSecondsRealtime(1f);

        float fallDuration = 1.5f; // Tiempo que va a durar la caída
        float timeElapsed = 0f;

        while (timeElapsed < fallDuration)
        {
            rb.velocity = new Vector2(rb.velocity.x, -fallSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;  // Espera hasta el siguiente frame
        }

        GameManager.gameManager.LoadScene(0);
        yield return new WaitForSecondsRealtime(2f);
        GameManager.gameManager.normalView = false;
    }
}