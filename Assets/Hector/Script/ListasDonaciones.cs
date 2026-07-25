using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ListasDonaciones
{
    public List<Donacion> donaciones = new List<Donacion>();
    public List<ListaCantidadDonaciones> listaCantidadDonaciones = new List<ListaCantidadDonaciones>();
    public int cantidadTotal;
    public int cantidadMaxima;
    public bool completado;

    public void addDonacion(Donacion donacion)
    {
        if (cantidadTotal >= cantidadMaxima)
        {
            return;
        }

        donaciones.Add(donacion);
        cantidadTotal++;
    }

    public void removeDonacion(Donacion donacion)
    {
        donaciones.Remove(donacion);
        cantidadTotal--;
    }
}
[System.Serializable]
public class ListaCantidadDonaciones
{
    public TipoDonacion donacionTipo;
    public int cantidad;

}
