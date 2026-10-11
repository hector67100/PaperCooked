using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject uiHoja;
    public Transform[] uiHojaHijos;
    public Animator anim;
    public GameObject dataUI;
    public GameObject dataUI2;
    public GameObject gameOverPanel;
    public Animator DataUIAnim;
    public Animator DataUIAnim2;
    public Animator Jugador1UIAnim;
    public ListasDonaciones lista;
    [SerializeField] private TMP_Text textoTiempo;
    [SerializeField] private TMP_Text textoPuntaje;
    [SerializeField] private TMP_Text textoCalificacion;
    public List<CanvasGroup> grupo;
    public GameObject ListaAConsiderar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        uiHojaHijos = new Transform[uiHoja.transform.childCount];
        for(int i = 0; i < uiHoja.transform.childCount; i++)
        {
            uiHojaHijos[i] = uiHoja.transform.GetChild(i);
        }

    }

    // Update is called once per frame

    public void CambiarA2()
    {
        Jugador1UIAnim.Play("1_A_2");
    }
    
    public void ActualizarHoja( bool mostrar = true)
    {
        
        int index = 0;
        foreach (ListaCantidadDonaciones child in lista.listaCantidadDonaciones)
        {
            uiHojaHijos[index].gameObject.SetActive(true);
            uiHojaHijos[index].Find("texto").GetComponent<TMPro.TextMeshProUGUI>().text = child.donacionTipo.ToString();
            uiHojaHijos[index].Find("num1").GetComponent<TMPro.TextMeshProUGUI>().text = "0";
            uiHojaHijos[index].Find("num2").GetComponent<TMPro.TextMeshProUGUI>().text = child.cantidad.ToString();
            index++;
        }

        if(mostrar)
        {
            anim.Play("INPagina",0,0.0f);
        }
    }

    public void ActualizarHojaCantidad(TipoDonacion tipo, int cantidad)
    {
        foreach (Transform child in uiHojaHijos)
        {
            
            if (child.gameObject.activeSelf && child.Find("texto").GetComponent<TMPro.TextMeshProUGUI>().text == tipo.ToString())
            {
                child.Find("num1").GetComponent<TMPro.TextMeshProUGUI>().text = cantidad.ToString();
                break;
            }
        }
    }

    public void SacarHoja()
    {
        anim.Play("OUTPagina");
        MusicMixed.index.Papel();
        foreach (Transform child in  uiHojaHijos)
        {
            child.gameObject.SetActive(false);
        }
        ActualizarHoja();
    }

    public void MostrarDataUI(bool mostrar = false, Sprite sprite = null, string texto = "", int playerIndex = 0)
    {
        GameObject ui = playerIndex == 0 ? dataUI : dataUI2;
        Animator animData = playerIndex == 0 ? DataUIAnim : DataUIAnim2;

        if (mostrar)
        {
            ui.GetComponentInChildren<UnityEngine.UI.Image>().sprite = sprite;
            ui.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = texto;

            animData.Play("INDatoUI");
        }
        else
        {
            animData.Play("OUTDatoUI");
        }
    }

    public void ActualizarTextoUI(float tiempoEnSegundos)
    {
        if (tiempoEnSegundos < 0) tiempoEnSegundos = 0;

        int minutos = Mathf.FloorToInt(tiempoEnSegundos / 60);
        int segundos = Mathf.FloorToInt(tiempoEnSegundos % 60);

        if (textoTiempo != null)
        {
            textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    public void MostrarGameOver(string puntaje = "", string calificacion = "")
    {
        if (textoPuntaje != null && puntaje != "")
        {
            textoPuntaje.text = puntaje;
        }

        if (textoCalificacion != null && calificacion != "")
        {
            textoCalificacion.text = calificacion;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void ResaltarRasgo(string nombreRasgo)
    {
        if (ListaAConsiderar == null) return;

        foreach (Transform child in ListaAConsiderar.transform)
        {
            if (child.gameObject.name.Trim().Equals(nombreRasgo.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                TMP_Text texto = child.GetComponentInChildren<TMP_Text>();
                if (texto != null)
                {
                    texto.color = Color.red;
                }
                else
                {
                    Debug.LogWarning("ResaltarRasgo: No se encontró TMP_Text en " + child.gameObject.name);
                }
                break;
            }
        }
    }
}
