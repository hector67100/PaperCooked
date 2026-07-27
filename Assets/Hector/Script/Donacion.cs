using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Donacion : MonoBehaviour
{
    public string nombre;
    public bool Estado;
    public TipoDonacion tipo;
    public Sprite Imagen;

   // public DonacionRasgo[] rasgos;
   
}

[System.Serializable]
public class DonacionRasgo
{
  
}

    public enum TipoDonacion
    {
        Comida,
        Ropa,
        Medicamento,
        Agua
    }