
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

    [Header("Lanzamiento")]
    [SerializeField] private float fuerzaLanzamiento = 15f;
    private bool isGamepad = false;
    private int cambioSelector = 0;

    private InputAction interactAction;
    private InputAction throwAction;
    private InputAction aimAction;
    private InputAction nextObjectAction;
    private InputAction previousObjectAction;

    private GameObject ObtenerObjetoMasCercano()
    {
        GameObject objetoMasCercano = null;
        float distanciaMinima = float.MaxValue;

        foreach (GameObject obj in donacionLista)
        {
            if (obj != null)
            {
                float distancia = Vector3.Distance(transform.position, obj.transform.position);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    objetoMasCercano = obj;
                }
            }
        }

        return objetoMasCercano;
    }

    public void SetGamepad(bool value)
    {
        isGamepad = value;
    }

    public void SetInputActions(InputAction interact, InputAction throwAct, InputAction aim, InputAction nextObj, InputAction prevObj)
    {
        interactAction = interact;
        throwAction = throwAct;
        aimAction = aim;
        nextObjectAction = nextObj;
        previousObjectAction = prevObj;
    }

    void Update()
    {
        // --- AGARRAR OBJETO ---
        if (interactAction != null && interactAction.WasPressedThisFrame())
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
                //    MusicMixed.index.Abrir();
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
                        // MusicMixed.index.Premio();
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
                // MusicMixed.index.Depositar();
                UIManager.instance.MostrarDataUI(false);
                objetoTomado = null;
            }

            if(puedeGuardar & objetoTomado != null)
            {
                guardadoObjeto.GuardarEnCaja(objetoTomado);
                // MusicMixed.index.Depositar();
                objetoTomado.SetActive(false);
                objetoTomado = null;
            }
            else if(puedeGuardar & objetoTomado == null & guardadoObjeto !=null)
            {
                // MusicMixed.index.Abrir();
                guardadoObjeto.SpawnearObjetosEnMesa();
            }
        }

        // --- ARROJAR OBJETO ---
        if (throwAction != null && throwAction.WasPressedThisFrame())
        {
            if (objetoTomado != null && objetoTomado.transform.parent == transform)
            {
                Vector2 direccionLanzamiento = Vector2.zero;

                if (isGamepad)
                {
                    // Vector2 aimInput = aimAction != null ? aimAction.ReadValue<Vector2>() : Vector2.zero;
                    // if (aimInput.sqrMagnitude > 0.01f)
                    // {
                    //     direccionLanzamiento = aimInput.normalized;
                    // }
                    // else
                    // {
                    //     direccionLanzamiento = spriteRenderer.flipX ? Vector2.left : Vector2.right;
                    // }
                }
                else
                {
                    Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
                    Vector3 mouseWorldPosition3D = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
                    Vector2 mouseWorldPosition = new Vector2(mouseWorldPosition3D.x, mouseWorldPosition3D.y);
                    direccionLanzamiento = (mouseWorldPosition - (Vector2)transform.position).normalized;
                }

                objetoTomado.transform.SetParent(null);

                if (objetoTomado.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.gravityScale = 0;
                    rb.AddForce(direccionLanzamiento * fuerzaLanzamiento, ForceMode2D.Impulse);
                }

                objetoTomado = null;
            }
        }

        if(nextObjectAction != null && nextObjectAction.WasPressedThisFrame())
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

        if(previousObjectAction != null && previousObjectAction.WasPressedThisFrame())
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
                if(!donacionLista.Contains(other.gameObject))
                {
                    donacionLista.Add(other.gameObject);
                }
                
                foreach (GameObject obj in donacionLista)
                {
                    if (obj != null && obj.GetComponent<DonacionSelector>() != null)
                    {
                        obj.GetComponent<DonacionSelector>().Apagar();
                    }
                }
                
                objetoTomar = ObtenerObjetoMasCercano();
                if (objetoTomar != null && objetoTomar.GetComponent<DonacionSelector>() != null)
                {
                    objetoTomar.GetComponent<DonacionSelector>().Activar();
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
        if(other.gameObject.CompareTag("Donacion"))
        {
            GameObject nuevoMasCercano = ObtenerObjetoMasCercano();
            
            if (nuevoMasCercano != objetoTomar)
            {
                if (objetoTomar != null && objetoTomar.GetComponent<DonacionSelector>() != null)
                {
                    objetoTomar.GetComponent<DonacionSelector>().Apagar();
                }
                
                objetoTomar = nuevoMasCercano;
                
                if (objetoTomar != null && objetoTomar.GetComponent<DonacionSelector>() != null)
                {
                    objetoTomar.GetComponent<DonacionSelector>().Activar();
                }
            }
        }
        
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
