
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject objetoTomar;
    public CajaDonacion caja;
    public bool puedeDonar = false;
    public bool eliminar = false;
    public GameObject Posicion_donacion;
    [SerializeField] TipoDonacion tipoDonacionPermitida;

    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private InputActionReference arrojarAction;

    [SerializeField] private float fuerzaLanzamiento = 15f; // Ajusta este valor según la masa del objeto

    private void OnEnable()
    {
        if (interactAction != null) interactAction.action.Enable();
        if (arrojarAction != null) arrojarAction.action.Enable();
    }

    private void OnDisable()
    {
        if (interactAction != null) interactAction.action.Disable();
        if (arrojarAction != null) arrojarAction.action.Disable();
    }

    void Update()
    {
        // --- AGARRAR OBJETO ---
        if (interactAction != null && interactAction.action.WasPressedThisFrame())
        {
            if (objetoTomar != null)
            {
                objetoTomar.transform.SetParent(Posicion_donacion.transform);
                objetoTomar.transform.localPosition = Vector3.zero;

                if (objetoTomar.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.linearVelocity = Vector2.zero;
                }
            }

            if(caja != null)
            {
                if(!caja.open)
                {
                   caja.AbrirCaja();
                }
                
            }

            if(puedeDonar && objetoTomar != null)
            {
                if(objetoTomar.TryGetComponent<Donacion>(out Donacion cajaDonacionEnvio))
                {
                    Debug.Log("Tipo de donación: " + cajaDonacionEnvio.tipo);
                    if(cajaDonacionEnvio.tipo == tipoDonacionPermitida)
                    {
                        GameManager.instance.AddDonacion(objetoTomar);
                        objetoTomar = null;
                    }
                    else
                    {
                        Debug.Log("Tipo de donación no permitido.");
                    }
                }
            }

            if(eliminar && objetoTomar != null)
            {
                Destroy(objetoTomar);
                objetoTomar = null;
            }
        }

        // --- ARROJAR OBJETO HACIA EL MOUSE ---
        if (arrojarAction != null && arrojarAction.action.WasPressedThisFrame())
        {
            // Verificamos que tengamos un objeto tomado y que sea nuestro hijo
            if (objetoTomar != null && objetoTomar.transform.parent == transform)
            {
                // 1. Obtener la posición del mouse en la pantalla con el nuevo Input System
                Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

                // 2. Convertir la posición de la pantalla a coordenadas del mundo 2D
                Vector3 mouseWorldPosition3D = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
                Vector2 mouseWorldPosition = new Vector2(mouseWorldPosition3D.x, mouseWorldPosition3D.y);

                // 3. Calcular la dirección normalizada desde el jugador hacia el mouse
                Vector2 direccionLanzamiento = (mouseWorldPosition - (Vector2)transform.position).normalized;

                // 4. Desvincular el objeto del jugador
                objetoTomar.transform.SetParent(null);

                // 5. Aplicar la fuerza
                if (objetoTomar.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.gravityScale = 0;
                    rb.AddForce(direccionLanzamiento * fuerzaLanzamiento, ForceMode2D.Impulse);
                }

                // 6. Soltar la referencia
                objetoTomar = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.tag)
        {
            case "Donacion":
                if (objetoTomar == null)
                {
                    objetoTomar = other.gameObject;
                }
            break;
            case "Caja":
                if (caja == null)
                {
                    caja = other.gameObject.GetComponent<CajaDonacion>();
                }
            break;
            case "Donar":
                puedeDonar = true;
                if(other.gameObject.GetComponent<CajaDonacionEnvio>() != null)
                {
                    tipoDonacionPermitida = other.gameObject.GetComponent<CajaDonacionEnvio>().tipoDonacion;
                }
                
            break;
            case "Basura":
                eliminar = true;
            break;

        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!puedeDonar && other.gameObject.CompareTag("Donar"))
        {

            puedeDonar = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch(other.gameObject.tag)
        {
            case "Donacion":
                if (objetoTomar == other.gameObject && objetoTomar.transform.parent != transform)
                {
                    objetoTomar = null;
                }
            break;
            case "Caja":
                caja = null;
            break;
            case "Donar":
                puedeDonar = false;
            break;
            case "Basura":
                eliminar = false;
            break;

        }
    }
}