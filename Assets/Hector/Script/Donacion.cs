using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Donacion
{
    public string nombre;
    public DonacionRasgo[] rasgos;
    public TipoDonacion tipo;
}

[System.Serializable]
public class DonacionRasgo
{
    public string rasgo;
    public bool esPositivo;
}

    public enum TipoDonacion
    {
        Comida,
        Ropa,
        Medicamento,
        Dinero,
        Juguete,
        Insumos
    }