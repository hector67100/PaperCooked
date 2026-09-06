
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject objetoTomar;
    private GameObject objetoTomado;
    public CajaDonacion caja;
    public bool puedeDonar = false;
    public bool eliminar = false;
    public bool puedeGuardar = false;
    public GameObject Posicion_donacion;
    [SerializeField] TipoDonacion tipoDonacionPermitida;
    [SerializeField] CajaGuardadoObjetos guardadoObjeto;
    HashSet<GameObject> donacionColisionada = new HashSet<GameObject>();
    public List<GameObject> donacionLista = new List<GameObject>();

    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private InputActionReference arrojarAction;

    [SerializeField] private InputActionReference proximoObjetoAction;
    [SerializeField] private InputActionReference anteriorObjetoAction;
    [SerializeField] private float fuerzaLanzamiento = 15f;
    private int cambioSelector = 0;

    private void OnEnable()
    {
        if (interactAction != null) interactAction.action.Enable();
        if (arrojarAction != null) arrojarAction.action.Enable();
        if (proximoObjetoAction != null) proximoObjetoAction.action.Enable();
        if (anteriorObjetoAction != null) anteriorObjetoAction.action.Enable();
    }

    private void OnDisable()
    {
        if (interactAction != null) interactAction.action.Disable();
        if (arrojarAction != null) arrojarAction.action.Disable();
        if (proximoObjetoAction != null) proximoObjetoAction.action.Disable();
        if (anteriorObjetoAction != null) anteriorObjetoAction.action.Disable();
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
                UIManager.instance.MostrarDataUI(true, objetoTomar.GetComponentInChildren<SpriteRenderer>().sprite, objetoTomar.GetComponent<Donacion>().rasgos.nombreRasgo);

                if (objetoTomar.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.linearVelocity = Vector2.zero;
                }
                
                objetoTomar.GetComponent<DonacionSelector>().Apagar();
                objetoTomado = objetoTomar;
            }

            if(caja != null && objetoTomar==null)
            {
                if(!caja.open)
                {
                   caja.AbrirCaja();
                }
                
            }

            if(puedeDonar && objetoTomado != null)
            {
                if(objetoTomado.TryGetComponent<Donacion>(out Donacion cajaDonacionEnvio))
                {

                    if(cajaDonacionEnvio.tipo == tipoDonacionPermitida)
                    {
                        GameManager.instance.AddDonacion(objetoTomado);
                        objetoTomado = null;
                        UIManager.instance.MostrarDataUI(false);
                    }
                    else
                    {
                        Debug.Log("Tipo de donación no permitido.");
                    }
                }
            }

            if(eliminar && objetoTomado != null)
            {
                Destroy(objetoTomado);
                objetoTomado = null;
            }

            if(puedeGuardar & objetoTomado != null)
            {
                guardadoObjeto.GuardarEnCaja(objetoTomado);
                objetoTomado.SetActive(false);
                objetoTomado = null;
            }
            else if(puedeGuardar & objetoTomado == null & guardadoObjeto !=null)
            {
                guardadoObjeto.SpawnearObjetosEnMesa();
            }
        }

        // --- ARROJAR OBJETO HACIA EL MOUSE ---
        if (arrojarAction != null && arrojarAction.action.WasPressedThisFrame())
        {
            // Verificamos que tengamos un objeto tomado y que sea nuestro hijo
            if (objetoTomado != null && objetoTomado.transform.parent == transform)
            {
                // 1. Obtener la posición del mouse en la pantalla con el nuevo Input System
                Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

                // 2. Convertir la posición de la pantalla a coordenadas del mundo 2D
                Vector3 mouseWorldPosition3D = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
                Vector2 mouseWorldPosition = new Vector2(mouseWorldPosition3D.x, mouseWorldPosition3D.y);

                // 3. Calcular la dirección normalizada desde el jugador hacia el mouse
                Vector2 direccionLanzamiento = (mouseWorldPosition - (Vector2)transform.position).normalized;

                // 4. Desvincular el objeto del jugador
                objetoTomado.transform.SetParent(null);

                // 5. Aplicar la fuerza
                if (objetoTomado.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.gravityScale = 0;
                    rb.AddForce(direccionLanzamiento * fuerzaLanzamiento, ForceMode2D.Impulse);
                }

                // 6. Soltar la referencia
                objetoTomado = null;
            }
        }

        if(proximoObjetoAction.action.WasPressedThisFrame())
        {
            cambioSelector++;
            cambioSelector = cambioSelector > donacionLista.Count-1 ? 0 : cambioSelector;
            if( donacionLista.Count > 1)
            {
                objetoTomar.GetComponent<DonacionSelector>().Apagar();
                objetoTomar = donacionLista[cambioSelector];
                objetoTomar.GetComponent<DonacionSelector>().Activar();
            }

        }

        if(anteriorObjetoAction.action.WasPressedThisFrame())
        {
           
            cambioSelector--;
            cambioSelector = cambioSelector < 0 ? donacionLista.Count-1 : cambioSelector;
            if( donacionLista.Count > 1)
            {
                objetoTomar.GetComponent<DonacionSelector>().Apagar();
                objetoTomar = donacionLista[cambioSelector];
                objetoTomar.GetComponent<DonacionSelector>().Activar();
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
                    objetoTomar.GetComponent<DonacionSelector>().Activar();
                }
                // donacionColisionada.Add(other.gameObject);
                // donacionLista = donacionColisionada.ToList();
                if(!donacionLista.Contains(other.gameObject))
                {
                    donacionLista.Add(other.gameObject);
                }
                
            break;
            case "Guardar":
             puedeGuardar = true;
             guardadoObjeto = other.gameObject.GetComponent<CajaGuardadoObjetos>();
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
                    objetoTomar.GetComponent<DonacionSelector>().Apagar();
                    objetoTomar = null;
                    // donacionColisionada.Remove(other.gameObject);
                    // donacionLista = donacionColisionada.ToList();
                   
                }
                 donacionLista.Remove(other.gameObject);
            break;
            case "Caja":
                caja = null;
            break;
            case "Guardar":
             puedeGuardar = false;
             guardadoObjeto = null;
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