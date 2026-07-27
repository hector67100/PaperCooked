using UnityEngine;

public class CajaDonacion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool open = false;
    public float tiempo = 0;
    public GameObject Interactuable;
    public Sprite Abierto, Cerrado;
    public SpriteRenderer Caja_img;

    public GameObject[] donacionesCaja;
    public ResetChildrenPosition aaaaa;

    public void AbrirCaja()
    {if (open)
            return;
        open = true;
        Caja_img.sprite = Abierto;
        Interactuable.SetActive(false);
        aaaaa.OrganizarHijos();
       /// GameManager.instance.aparecerDonaciones(donacionesCaja);
   
    }
    public bool Cronometro(out float T, float tempo, float reset)
    {
        T = tempo + Time.deltaTime;
        if (T > reset)
        {        T = 0;
            return true;
    }
        return false;
    }
    private void Update()
    {
        if (open)
        {
            if (Cronometro(out tiempo, tiempo, 15))
            {
                open = false;
                Interactuable.SetActive(true);
                Caja_img.sprite = Cerrado;
            }
        }
    }
}
