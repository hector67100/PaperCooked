using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    public ListasDonaciones[] listasDonaciones;
    public ListasDonaciones listasDonacionActiva;
    public ListasDonaciones[] listasDonacionesTerminadas;
    public GameObject mesa;
    public int donacionesHechas = 0;
    public bool juegoTerminado = false;
    public int NumeroDeListas = 4;

    private float Tiempo = 500;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        GenerarListas();
        
        listasDonacionActiva = listasDonaciones[0];
        UIManager.instance.lista = listasDonacionActiva;
        UIManager.instance.ActualizarHoja(false);
        
    }

    void Update()
    {
        if(!juegoTerminado)
        {
            if (Tiempo > 0)
            {
                Tiempo -= Time.deltaTime;
            }
            else
            {
                Tiempo = 0;
            }
            
            UIManager.instance.ActualizarTextoUI(Tiempo);   
        }
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


    public void GenerarListas()
    {
        List<ListasDonaciones> listaD = new List<ListasDonaciones>();
        // List<ListaCantidadDonaciones> CantidadesLista = new List<ListaCantidadDonacione>();
        int cantidadDonacionesMaximo = 0;
        for(int i = 0; i<NumeroDeListas; i++)
        {
            int CantidadDeDonacions = Random.Range(1, 4);
            ListasDonaciones donacion = new ListasDonaciones();

            for(int j=0; j< CantidadDeDonacions; j++)
            {
                ListaCantidadDonaciones nueva = new ListaCantidadDonaciones();
                TipoDonacion tipo;
                do
                {
                    tipo = GetTipoDonacion( Random.Range(0, 3));
                }while(donacion.TipoEnLista(tipo));

                nueva.donacionTipo = tipo;
                nueva.cantidad =  Random.Range(1, 5);
                cantidadDonacionesMaximo += nueva.cantidad;
                donacion.listaCantidadDonaciones.Add(nueva);
            }

            // donacion.listaCantidadDonaciones = CantidadesLista;
            donacion.cantidadMaxima = cantidadDonacionesMaximo + 2;
            donacion.completado = false;
            cantidadDonacionesMaximo = 0;
            listaD.Add(donacion);
            // CantidadesLista.Clear();
            
        }

        listasDonaciones = listaD.ToArray();
        
    }

    public TipoDonacion GetTipoDonacion(int num)
    {
        TipoDonacion donaciontipo = TipoDonacion.Agua;
        switch(num)
        {
            case 0:
            donaciontipo = TipoDonacion.Comida;
            break;
            case 1:
            donaciontipo = TipoDonacion.Ropa;
            break;
            case 2:
            donaciontipo = TipoDonacion.Medicamento;
            break;
            case 3:
            break;

        }

        return donaciontipo;
    }
    
}
