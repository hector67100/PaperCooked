using UnityEngine;
using UnityEngine.UI;

public class Gestionar_panel_Music : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject icono_Apagada, icono_Encendido;
    public string Evaluar = "";
    public bool OnEnablethis = false;
   
    void OnEnable()
    {if (!OnEnablethis)
            return;
        if (PlayerPrefs.GetInt(Evaluar, 1) > 0)
        {
            icono_Apagada.SetActive(false);
            icono_Encendido.SetActive(true);
           
        }
        else
        {
            icono_Apagada.SetActive(true);
            icono_Encendido.SetActive(false);
        }
    }
    public void ApagarEncenderMusic()
    {
        if (PlayerPrefs.GetInt(Evaluar, 1) > 0)
        {
            PlayerPrefs.SetInt(Evaluar, 0);
            icono_Apagada.SetActive(true);
            icono_Encendido.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt(Evaluar, 1);
            icono_Apagada.SetActive(false);
            icono_Encendido.SetActive(true);
        }
        MusicMixed.index.StablecerVolumen();

    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
