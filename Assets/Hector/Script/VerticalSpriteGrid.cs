using UnityEngine;

[ExecuteAlways] // Funciona también en el editor sin darle a Play
public class VerticalSpriteGrid : MonoBehaviour
{
    [Header("Configuración de la Columna")]
    [Tooltip("Espacio extra opcional entre cada sprite (puede ser 0 o negativo)")]
    public float padding = 0.2f;

    void Start()
    {
        OrganizeVertical();
    }

    [ContextMenu("Organizar Sprites")]
    public void OrganizeVertical()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        float currentY = 0f;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();

            if (sr == null)
            {
                Debug.LogWarning($"El hijo {child.name} no tiene SpriteRenderer.");
                continue;
            }

            // Obtenemos el alto total del sprite en unidades de mundo
            float spriteHeight = sr.bounds.size.y;
            float halfHeight = spriteHeight / 2f;

            // Para que la orilla SUPERIOR del sprite coincida con 'currentY',
            // bajamos su centro una distancia igual a la mitad de su alto.
            float targetY = currentY - halfHeight;

            // Asignamos la posición manteniendo X y Z locales intactos
            child.localPosition = new Vector3(0f, targetY, child.localPosition.z);

            // Preparamos el punto de inicio para el SIGUIENTE sprite
            // (Avanzamos el alto completo del sprite actual + la separación/padding)
            currentY -= (spriteHeight + padding);
        }
    }

    // Se ejecuta automáticamente en el Editor cuando cambias valores en el Inspector
    private void OnValidate()
    {
        OrganizeVertical();
    }
}