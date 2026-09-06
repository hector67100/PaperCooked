using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Donacion : MonoBehaviour
{
    public string nombre;
    public bool Estado;
    public TipoDonacion tipo;
    public Sprite Imagen;
    public DonacionRasgo rasgos = null;
   
}

[System.Serializable]
public class DonacionRasgo
{
  public bool negativo;
  public string nombreRasgo;
}

public enum TipoDonacion
{
    Comida,
    Ropa,
    Medicamento,
    Agua
}

[System.Serializable]
public class RasgoAsignador
{
    public TipoDonacion donacionTipo;
    public DonacionRasgo[] rasgos;
}