using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject uiHoja;
    public Transform[] uiHojaHijos;
    public Animator anim;
    public GameObject dataUI;
    public GameObject gameOverPanel;
    public Animator DataUIAnim;
    public ListasDonaciones lista;
    [SerializeField] private TMP_Text textoTiempo;

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

    public void MostrarDataUI(bool mostrar = false, Sprite sprite = null, string texto = "")
    {
        if(mostrar)
        {
            dataUI.GetComponentInChildren<UnityEngine.UI.Image>().sprite = sprite;
            dataUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = texto;

            DataUIAnim.Play("INDatoUI");
        }
        else
        {
            DataUIAnim.Play("OUTDatoUI");
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

    public void MostrarGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
