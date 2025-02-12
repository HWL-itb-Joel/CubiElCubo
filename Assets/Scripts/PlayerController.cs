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

    [Header("Win")]
    public Transform DoorTransform;
    public float rotationSpeed = 100f; // Velocidad de rotación mientras el jugador gira
    public float shrinkDuration = 2f;  // Tiempo en el que el jugador se hace más pequeño (y desaparece)
    private bool isMoving = false;  // Para evitar que se mueva mientras gira
    private bool isShrinking = false;  // Para saber si el jugador está reduciendo su tamaño
    private float shrinkTime = 0f;

    private void Start()
    {
        isDead = false;
        conoCollected = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
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
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);

            if (rb.velocity.x != 0)
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

        if (collision.CompareTag("puerta"))
        {
            StartCoroutine(Win());
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

    IEnumerator Win()
    {
        rb.simulated = false;

        while (Vector2.Distance(transform.position, DoorTransform.position) != 0f)
        {
            transform.position = Vector2.MoveTowards(transform.position, DoorTransform.position, (moveSpeed / 2) * Time.deltaTime);
            yield return null;
        }

        float rotationTime = 2f;  // Tiempo de rotación (2 segundos)
        float elapsedTime = 0f;

        // El jugador empieza a girar mientras se reduce
        while (elapsedTime < rotationTime)
        {
            // Rotar el jugador
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            // Reducir el tamaño del jugador mientras gira
            float shrinkFactor = Mathf.Lerp(1f, 0f, elapsedTime / shrinkDuration); // Hacemos que se haga más pequeño con el tiempo
            transform.localScale = new Vector3(shrinkFactor, shrinkFactor, shrinkFactor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Aseguramos que al final el personaje esté completamente desaparecido
        transform.localScale = new Vector3(0f, 0f, 0);

        // Aquí puedes desactivar el jugador o realizar otras acciones si lo necesitas
        gameObject.SetActive(false);  // Opcional: Desactivar el jugador

        // Puedes agregar un tiempo antes de que el jugador reaparezca o se reinicie
        yield return new WaitForSeconds(1f);
    }
}