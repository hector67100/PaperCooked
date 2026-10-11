using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

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
    private float Tiempo = 240;
    public List<RasgoAsignador> rasgos = new List<RasgoAsignador>();
    public List<int> objetos = new List<int> {{0},{0},{0},{0}};
    public List<GameObject> donaciones = new List<GameObject>();
    public List<GameObject>[] CajasDonaciones;
    public int CajaAUsar = 0;
    public int NumeroItems = 0;
    public int NumeroItemsInvocado = 0;

    [Header("Puntaje")]
    public int puntaje = 0;
    public int rachaActual = 0;
    public int mejorRacha = 0;
    [SerializeField] private int puntosPorDonacion = 100;
    [SerializeField] private int bonusPorRacha = 50;

    [Header("Deadend")]
    [SerializeField] private CajaGuardadoObjetos cajaGuardado;

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
        GenerarCajas();
        juegoTerminado = false;
        puntaje = 0;
        rachaActual = 0;
        mejorRacha = 0;
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
                juegoTerminado = true;
                UIManager.instance.MostrarGameOver(puntaje.ToString(), CalcularCalificacion());
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

    public void VerificarListaActual()
    {
        if (juegoTerminado) return;

        if (HaySuficientesObjetos()) return;

        // La lista actual ya no se puede cumplir, buscar la siguiente
        Debug.Log("Lista actual (" + donacionesHechas + ") imposible, buscando siguiente...");
        CambiarDonacion();
    }

    public void CambiarDonacion()
    {
        donacionesHechas++;

        while (donacionesHechas < listasDonaciones.Length)
        {
            listasDonacionActiva = listasDonaciones[donacionesHechas];

            if (HaySuficientesObjetos())
            {
                UIManager.instance.lista = listasDonacionActiva;
                UIManager.instance.SacarHoja();
                return;
            }

            donacionesHechas++;
        }

        // No quedan listas cumplibles
        Debug.Log("DEADEND: No se encontró lista cumplible. CajaAUsar=" + CajaAUsar + " TotalCajas=" + (CajasDonaciones != null ? CajasDonaciones.Length : 0));
        juegoTerminado = true;
        UIManager.instance.MostrarGameOver(puntaje.ToString(), CalcularCalificacion());
    }

    public bool HaySuficientesObjetos()
    {
        foreach (ListaCantidadDonaciones requisito in listasDonacionActiva.listaCantidadDonaciones)
        {
            int yaDonados = listasDonacionActiva.donaciones.Count(d => d.tipo == requisito.donacionTipo);
            int faltan = requisito.cantidad - yaDonados;
            if (faltan <= 0) continue;

            int disponibles = ContarDisponibles(requisito.donacionTipo);

            Debug.Log("DEADEND check: tipo=" + requisito.donacionTipo + " faltan=" + faltan + " disponibles=" + disponibles);

            if (disponibles < faltan)
                return false;
        }

        return true;
    }

    private int ContarDisponibles(TipoDonacion tipo)
    {
        int total = 0;

        // Cajas pendientes por abrir
        if (CajasDonaciones != null)
        {
            for (int i = CajaAUsar; i < CajasDonaciones.Length; i++)
            {
                if (CajasDonaciones[i] == null) continue;
                total += CajasDonaciones[i].Count(obj => obj != null && obj.GetComponent<Donacion>().tipo == tipo);
            }
        }

        // Objetos guardados (dentro de la caja)
        if (cajaGuardado != null)
        {
            total += cajaGuardado.objetosAGuardar.Count(obj => obj != null && obj.GetComponent<Donacion>().tipo == tipo);

            // Objetos en la mesa del guardado (ya sacados de la caja)
            if (cajaGuardado.Mesa != null)
            {
                foreach (Transform spawn in cajaGuardado.Mesa.transform)
                {
                    if (spawn.tag == "Guardar" || spawn.tag == "Mantel") continue;
                    foreach (Transform item in spawn)
                    {
                        if (!item.gameObject.activeSelf) continue;
                        Donacion d = item.GetComponent<Donacion>();
                        if (d != null && d.tipo == tipo) total++;
                    }
                }
            }
        }

        // Objetos en la mesa (spawns activos)
        if (mesa != null)
        {
            int mesaCount = 0;
            foreach (Transform child in mesa.transform)
            {
                if (child.tag == "Caja" || child.tag == "Mantel") continue;
                foreach (Transform item in child)
                {
                    if (!item.gameObject.activeSelf) continue;
                    Donacion d = item.GetComponent<Donacion>();
                    if (d != null && d.tipo == tipo) { total++; mesaCount++; }
                }
            }
            Debug.Log("ContarDisponibles mesa=" + mesaCount + " tipo=" + tipo + " mesaName=" + mesa.name);
        }
        else
        {
            Debug.Log("ContarDisponibles: mesa es NULL");
        }

        return total;
    }

    public void AddDonacion(GameObject objeto)
    {
        listasDonacionActiva.addDonacion(objeto);
    }

    public void RegistrarDonacionCorrecta()
    {
        int bonus = rachaActual * bonusPorRacha;
        puntaje += puntosPorDonacion + bonus;
        rachaActual++;
        if (rachaActual > mejorRacha) mejorRacha = rachaActual;
    }

    public void RegistrarDonacionIncorrecta()
    {
        rachaActual = 0;
    }

    public string CalcularCalificacion()
    {
        float ratio = (float)donacionesHechas / NumeroDeListas;
        if (ratio >= 1f) return "S";
        if (ratio >= 0.75f) return "A";
        if (ratio >= 0.5f) return "B";
        if (ratio >= 0.25f) return "C";
        return "D";
    }


    public void GenerarListas()
    {
        List<ListasDonaciones> listaD = new List<ListasDonaciones>();
        // List<ListaCantidadDonaciones> CantidadesLista = new List<ListaCantidadDonacione>();
        int cantidadDonacionesMaximo = 0;
        for(int i = 0; i<NumeroDeListas; i++)
        {
            int CantidadDeDonacions = Random.Range(1, 2);
            ListasDonaciones donacion = new ListasDonaciones();

            for(int j=0; j< CantidadDeDonacions; j++)
            {
                ListaCantidadDonaciones nueva = new ListaCantidadDonaciones();
                TipoDonacion tipo;
                int tipoNum = 0;
                do
                {
                    tipoNum = Random.Range(0, 4);
                    tipo = GetTipoDonacion(tipoNum);
                }while(donacion.TipoEnLista(tipo));

                nueva.donacionTipo = tipo;
                nueva.cantidad =  Random.Range(1, 3);
                objetos[tipoNum] += nueva.cantidad;
                NumeroItems++;
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
            donaciontipo = TipoDonacion.Agua;
            break;

        }

        return donaciontipo;
    }

    public DonacionRasgo AsignarRasgo(TipoDonacion tipo, bool positivo)
    {
        DonacionRasgo donacion;
        RasgoAsignador rasgoAAsignar = rasgos.Find(x => x.donacionTipo == tipo );
        List<DonacionRasgo> rasosObetenidos = positivo == true ? rasgoAAsignar.rasgos.ToList().FindAll(x =>  x.negativo == false) : rasgoAAsignar.rasgos.ToList().FindAll(x =>  x.negativo == true);

        while(rasosObetenidos.Count < 1)
        {
            rasgoAAsignar = rasgos.Find(x => x.donacionTipo == GetTipoDonacion(Random.Range(0,2)));
            rasosObetenidos = positivo == true ? rasgoAAsignar.rasgos.ToList().FindAll(x =>  x.negativo == false) : rasgoAAsignar.rasgos.ToList().FindAll(x =>  x.negativo == true);
        }

        int numero;
        
        if(rasosObetenidos.Count > 1)
        {
            numero = Random.Range(0,  rasosObetenidos.Count);
        }
        else
        {
            numero = 0;
        }

        donacion = rasosObetenidos[numero];
        
        return donacion;
    }

    void GenerarCajas()
    {
        List<GameObject> objetoCajas = new List<GameObject>();
        TipoDonacion tipoAAparecer;
        List<GameObject> donacionesListas;
        GameObject objetoInvocado;
        for(int i=0; i< objetos.Count;i++)
        {
           tipoAAparecer = GetTipoDonacion(i);
           donacionesListas = donaciones.FindAll(x => x.GetComponent<Donacion>().tipo == tipoAAparecer && x.GetComponent<Donacion>().rasgos.nombreRasgo == "");
           for(int j = 0; j< objetos[i]; j++)
           {
              
              objetoInvocado = Instantiate(donacionesListas[Random.Range(0,donacionesListas.Count)],new Vector3(0,0,0), Quaternion.identity);
              objetoInvocado.GetComponent<Donacion>().rasgos = AsignarRasgo(tipoAAparecer,true);
              objetoCajas.Add(objetoInvocado);
              objetoInvocado.SetActive(false);
              NumeroItemsInvocado++;
              
           }
        }

        int bulto = objetoCajas.Count/2;

        for(int i=0; i< bulto;i++)
        {
  
            tipoAAparecer = GetTipoDonacion(Random.Range(0,3));
            donacionesListas = donaciones.FindAll(x => x.GetComponent<Donacion>().tipo == tipoAAparecer);
            objetoInvocado = Instantiate(donacionesListas[Random.Range(0,donacionesListas.Count)],new Vector3(0,0,0), Quaternion.identity);
            objetoInvocado.GetComponent<Donacion>().rasgos = objetoInvocado.GetComponent<Donacion>().rasgos.nombreRasgo == "" ? AsignarRasgo(tipoAAparecer,false) : objetoInvocado.GetComponent<Donacion>().rasgos;
            objetoCajas.Add(objetoInvocado);
            objetoInvocado.SetActive(false);
            NumeroItemsInvocado++;
        }

        Mezclar(objetoCajas);
        int cajas= objetoCajas.Count % 6 > 0 ? Mathf.CeilToInt(objetoCajas.Count/6) + 1 : Mathf.CeilToInt(objetoCajas.Count/6);
        CajasDonaciones = new List<GameObject>[cajas];
        int countCaja = 0;
        int numCaja = 0;
        CajasDonaciones[numCaja] = new List<GameObject>();

        for(int i=0;i<objetoCajas.Count;i++)
        {
            if(countCaja<6)
            {
                CajasDonaciones[numCaja].Add(objetoCajas[i]);
                countCaja++;
            }
            else
            {

                countCaja=0;
                numCaja++;
                if(numCaja<CajasDonaciones.Length)
                {
                    CajasDonaciones[numCaja] = new List<GameObject>();
                    CajasDonaciones[numCaja].Add(objetoCajas[i]);
                }

            }
            
        }
    }

    public void Mezclar<T>(List<T> lista)
    {
        System.Random rnd = new System.Random();
        int n = lista.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            T valor = lista[k];
            lista[k] = lista[n];
            lista[n] = valor;
        }
    }
    
}
