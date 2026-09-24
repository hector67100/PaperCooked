using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Variables de Dirección (Int)")]
    public int movimientoX;
    public int movimientoY;

    [Header("Opciones de Orientación")]
    [Tooltip("Marca TRUE si quieres que el sprite se volatee horizontalmente (Flip X). FALSE si prefieres rotar la escala.")]
    [SerializeField] private bool useSpriteRendererFlip = true;

    public Rigidbody2D rb;
    private Vector2 movementInput;
    public SpriteRenderer spriteRenderer;
    public Animator anim;

    private InputAction moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rb.gravityScale = 0f;
    }

    public void SetMoveAction(InputAction action)
    {
        moveAction = action;
    }

    private void Update()
    {
        Vector2 input = Vector2.zero;
        if (moveAction != null)
        {
            input = moveAction.ReadValue<Vector2>();
        }

        movimientoX = Mathf.RoundToInt(input.x);
        movimientoY = Mathf.RoundToInt(input.y);

        movementInput = new Vector2(movimientoX, movimientoY).normalized;

        UpdateAnimator();
        HandleSpriteFacing(movimientoX);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    private void UpdateAnimator()
    {
        if (anim == null) return;

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
