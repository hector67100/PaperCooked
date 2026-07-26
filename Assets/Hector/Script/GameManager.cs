using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    public ListasDonaciones[] listasDonaciones;
    public ListasDonaciones listasDonacionActiva;
    public GameObject donacionesTest;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

         listasDonacionActiva = listasDonaciones[0];
    }

    public void Test()
    {
        listasDonacionActiva.addDonacion(donacionesTest);
    }

    public void TestCambiar()
    {
        listasDonacionActiva = listasDonaciones[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
