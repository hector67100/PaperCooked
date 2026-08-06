using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject uiHoja;
    public Transform[] uiHojaHijos;
    public Animator anim;
    public ListasDonaciones lista;
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
            anim.Play("INPagina");
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

        foreach (Transform child in  uiHojaHijos)
        {
            child.gameObject.SetActive(false);
        }
        ActualizarHoja();
    }
}
