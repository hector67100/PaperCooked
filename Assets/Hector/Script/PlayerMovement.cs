using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Variables de Dirección (Int)")]
    public int movimientoX;
    public int movimientoY;

    [Header("Opciones de Orientación")]
    [Tooltip("Marca TRUE si quieres que el sprite se voltee horizontalmente (Flip X). FALSE si prefieres rotar la escala.")]
    [SerializeField] private bool useSpriteRendererFlip = true;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Desactivamos la gravedad para movimiento libre en 2D
        rb.gravityScale = 0f;
    }

    private void Update()
    {
        // 1. Obtener entradas enteras (-1, 0, 1) para cada eje
        movimientoX = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        movimientoY = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        // 2. Crear el vector de movimiento y normalizarlo
        movementInput = new Vector2(movimientoX, movimientoY).normalized;

        // 3. ACTUALIZAR EL ANIMATOR
        UpdateAnimator();

        // 4. Controlar la orientación del Sprite (Mirar izquierda / derecha)
        HandleSpriteFacing(movimientoX);
    }

    private void FixedUpdate()
    {
        // Aplicamos el movimiento en el Rigidbody2D
        rb.linearVelocity = movementInput * moveSpeed;
    }

    private void UpdateAnimator()
    {
        if (anim == null) return;

        // Enviamos los valores enteros al Animator
        anim.SetInteger("MovimientoX", movimientoX);
        anim.SetInteger("MovimientoY", movimientoY);
    }

    private void HandleSpriteFacing(int inputX)
    {
        if (inputX == 0) return;

        if (useSpriteRendererFlip && spriteRenderer != null)
        {
            spriteRenderer.flipX = inputX < 0;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (inputX < 0 ? -1f : 1f);
            transform.localScale = scale;
        }
    }
}