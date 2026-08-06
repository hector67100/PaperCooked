using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    public ListasDonaciones[] listasDonaciones;
    public ListasDonaciones listasDonacionActiva;
    public ListasDonaciones[] listasDonacionesTerminadas;
    public GameObject mesa;
    public int donacionesHechas = 0;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        listasDonacionActiva = listasDonaciones[0];
        UIManager.instance.lista = listasDonacionActiva;
        UIManager.instance.ActualizarHoja(false);
    }

    public void aparecerDonaciones(GameObject[] lista)
    {
        foreach(GameObject donacion in lista)
        {
            GameObject objeto = Instantiate(donacion, new Vector3(0f, 0, 0), Quaternion.identity);
            objeto.transform.SetParent(mesa.transform);
        }

        mesa.GetComponent<ResetChildrenPosition>().OrganizarHijos();
    }

    public void CambiarDonacion()
    {
        donacionesHechas++;

        if(donacionesHechas < listasDonaciones.Length)
        {
            listasDonacionActiva = listasDonaciones[donacionesHechas];
            UIManager.instance.lista =listasDonaciones[donacionesHechas];
            UIManager.instance.SacarHoja();
        }   
    }

    public void AddDonacion(GameObject objeto)
    {
        listasDonacionActiva.addDonacion(objeto);
    }

    
}
