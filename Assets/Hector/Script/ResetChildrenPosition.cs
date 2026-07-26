using UnityEngine;

public class ResetChildrenPosition : MonoBehaviour
{
[Header("Referencia de Inicio")]
    [Tooltip("Objeto que define la posición desde donde empezará la lista.")]
    [SerializeField] private Transform spawnPoint;

    [Header("Ajustes de Espaciado")]
    [SerializeField] private float distanciaY = 0.2f;

    [ContextMenu("Organizar Hijos en Y")]
    public void OrganizarHijos()
    {
        // Determinamos el Y inicial: la Y local del spawnPoint (si existe) o 0f
        float startY = 0f;

        if (spawnPoint != null)
        {
            // Convertimos la posición del spawnPoint a espacio local respecto a este padre
            startY = transform.InverseTransformPoint(spawnPoint.position).y;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Si el propio spawnPoint es un hijo de este objeto, lo ignoramos para no moverlo
            if (spawnPoint != null && child == spawnPoint)
            {
                continue;
            }

            // El primer hijo queda en la Y del spawnPoint, y los siguientes van bajando
            float yPos = startY - (i * distanciaY);
            child.localPosition = new Vector3(0f, yPos, 0f);
        }

        Debug.Log($"Se alinearon los hijos verticalmente desde el Spawn Point.");
    }

    private void Start()
    {
        // Opcional: Descomenta esta línea si quieres que lo haga automáticamente al iniciar el juego.
        OrganizarHijos();
    }
}