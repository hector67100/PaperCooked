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

    public void addDonacion(GameObject donacion)
    {
        if (cantidadTotal >= cantidadMaxima)
        {
            return;
        }

        Donacion donacionComponent = donacion.GetComponent<Donacion>();

        donaciones.Add(donacionComponent);
        donacion.SetActive(false);
        donacion.transform.SetParent(null);
        cantidadTotal++;
        ListaCantidadDonacionesRevisar();
    }

    public void removeDonacion(GameObject donacion)
    {
        Donacion donacionComponent = donacion.GetComponent<Donacion>();
        donaciones.Remove(donacionComponent);
        cantidadTotal--;
    }

    public void ListaCantidadDonacionesRevisar()
    {
        bool completado = true;
        foreach (ListaCantidadDonaciones lista in listaCantidadDonaciones)
        {
            int cantidad = 0;
            foreach (Donacion donacion in donaciones)
            {
                if (donacion.tipo == lista.donacionTipo)
                {
                    cantidad++;
                }
            }
            
            UIManager.instance.ActualizarHojaCantidad(lista.donacionTipo, cantidad);
            if(cantidad < lista.cantidad)
            {
                completado = false;
            }
        }

        this.completado = completado;

        if(cantidadTotal == cantidadMaxima)
        {
            this.completado = true;
        }

        if(this.completado)
        {
            GameManager.instance.CambiarDonacion();
        }
    }

    public void ListaCantidadDonacionesRemover(TipoDonacion tipo)
    {
        ListaCantidadDonaciones lista = listaCantidadDonaciones.Find(x => x.donacionTipo == tipo);
        if (lista != null)
        {
            lista.cantidad--;
        }

    }


}
[System.Serializable]
public class ListaCantidadDonaciones
{
    public TipoDonacion donacionTipo;
    public int cantidad;

}
