using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Opciones de Orientación")]
    [Tooltip("Marca TRUE si quieres que el sprite se voltee horizontalmente (Flip X). FALSE si prefieres rotar la escala.")]
    [SerializeField] private bool useSpriteRendererFlip = true;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Desactivamos la gravedad para que el personaje no se caiga al moverse en Y
        rb.gravityScale = 0f;
    }

    private void Update()
    {
        // 1. Obtener entradas en los ejes X e Y (Teclas W/A/S/D, Flechas o Stick analógico)
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // Normalizamos el vector para evitar que el jugador se mueva más rápido en diagonal
        movementInput = new Vector2(inputX, inputY).normalized;

        // 2. Controlar la orientación del Sprite (Mirar izquierda / derecha)
        HandleSpriteFacing(inputX);
    }

    private void FixedUpdate()
    {
        // Aplicamos el movimiento directamente en el Rigidbody2D mediante física
        rb.linearVelocity = movementInput * moveSpeed;
    }

    private void HandleSpriteFacing(float inputX)
    {
        if (inputX == 0f) return;

        if (useSpriteRendererFlip && spriteRenderer != null)
        {
            // Método 1: Flip con el SpriteRenderer
            spriteRenderer.flipX = inputX < 0f;
        }
        else
        {
            // Método 2: Invirtiendo la escala en X (útil si el personaje tiene objetos hijos)
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (inputX < 0f ? -1f : 1f);
            transform.localScale = scale;
        }
    }
}