using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("Detección de suelo")]
    [SerializeField] private Transform groundCheck; // Punto en los pies
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isGrounded;

    [Header("Input System")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction jumpAction;

    // Escala original para evitar deformación al girar
    private Vector3 originalScale;

    [Header("Mensaje límite")]
    public GameObject mensajeUI; 
    public Vector3 mensajeOffset = new Vector3(0, 2, 0); // distancia sobre la cabeza

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        if (moveAction == null)
        {
            moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
        }

        if (jumpAction == null)
        {
            jumpAction = new InputAction("Jump", binding: "<Keyboard>/space");
        }

        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveInput = Vector2.zero;
        jumpAction.performed += ctx => Jump();

        if (mensajeUI != null) mensajeUI.SetActive(false);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        // Voltear sprite según dirección
        if (moveInput.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    private void Update()
    {
        // Comprobar si estamos en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsJumping", !isGrounded);

        // Animación de velocidad
        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));

        // Posicionar mensaje sobre la cabeza si está activo
        if (mensajeUI != null && mensajeUI.activeSelf)
        {
            mensajeUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + mensajeOffset);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }
    }

  
    // Límite con objeto LeftLimit

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LeftLimit"))
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (mensajeUI != null) mensajeUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LeftLimit"))
        {
            if (mensajeUI != null) mensajeUI.SetActive(false);
        }
    }
}
