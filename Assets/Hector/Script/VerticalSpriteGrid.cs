using UnityEngine;

[ExecuteAlways]
public class VerticalSpriteGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 1f;

    [Header("Orientation")]
    [Tooltip("Define si el grid se genera en el plano XY (2D/Side-scroll) o XZ (Top-down/3D)")]
    [SerializeField] private bool useXZPlane = false;

    [Header("Debug / Visuals")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color gridColor = Color.cyan;

    private int[,] gridArray;

    private void Awake()
    {
        gridArray = new int[width, height];
    }

    /// <summary>
    /// Obtiene la posición en el mundo real de la celda (x, y) respetando la posición, 
    /// rotación y escala del GameObject local.
    /// </summary>
    public Vector3 GetWorldPosition(int x, int y)
    {
        Vector3 localPos = useXZPlane 
            ? new Vector3(x * cellSize, 0f, y * cellSize) 
            : new Vector3(x * cellSize, y * cellSize, 0f);

        // Convierte la posición local a posición global considerando el Transform actual
        return transform.TransformPoint(localPos);
    }

    /// <summary>
    /// Convierte una posición en el mundo a coordenadas (x, y) de la matriz del grid.
    /// </summary>
    public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        // Convierte la posición del mundo a espacio local de este GameObject
        Vector3 localPos = transform.InverseTransformPoint(worldPosition);

        if (useXZPlane)
        {
            x = Mathf.FloorToInt(localPos.x / cellSize);
            y = Mathf.FloorToInt(localPos.z / cellSize);
        }
        else
        {
            x = Mathf.FloorToInt(localPos.x / cellSize);
            y = Mathf.FloorToInt(localPos.y / cellSize);
        }
    }

    /// <summary>
    /// Ajusta cualquier punto del mundo al centro de la celda más cercana en el grid.
    /// </summary>
    public Vector3 SnapToGrid(Vector3 worldPosition)
    {
        GetGridPosition(worldPosition, out int x, out int y);

        // Si la celda está dentro del rango del grid, devolvemos su centro
        if (IsValidCell(x, y))
        {
            Vector3 cellOrigin = GetWorldPosition(x, y);
            Vector3 offset = useXZPlane 
                ? transform.TransformDirection(new Vector3(cellSize / 2f, 0f, cellSize / 2f)) 
                : transform.TransformDirection(new Vector3(cellSize / 2f, cellSize / 2f, 0f));

            return cellOrigin + offset;
        }

        return worldPosition; // Retorna la original si está fuera del grid
    }

    public bool IsValidCell(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = gridColor;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 current = GetWorldPosition(x, y);

                if (useXZPlane)
                {
                    Vector3 nextX = GetWorldPosition(x + 1, y);
                    Vector3 nextZ = GetWorldPosition(x, y + 1);

                    Gizmos.DrawLine(current, nextX);
                    Gizmos.DrawLine(current, nextZ);
                }
                else
                {
                    Vector3 nextX = GetWorldPosition(x + 1, y);
                    Vector3 nextY = GetWorldPosition(x, y + 1);

                    Gizmos.DrawLine(current, nextX);
                    Gizmos.DrawLine(current, nextY);
                }
            }
        }

        // Líneas para cerrar los bordes del grid
        if (useXZPlane)
        {
            Gizmos.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height));
            Gizmos.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height));
        }
        else
        {
            Gizmos.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height));
            Gizmos.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height));
        }
    }
}