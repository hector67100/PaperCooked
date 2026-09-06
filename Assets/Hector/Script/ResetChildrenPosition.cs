
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ResetChildrenPosition : MonoBehaviour
{
    public List<GameObject> Spawns;
    public List<GameObject> Articulos;

    [Header("Referencia de Inicio")]
    [Tooltip("Objeto que define la posición desde donde empezará la lista.")]
    [SerializeField] private Transform spawnPoint;

    [Header("Ajustes de Espaciado")]
    [SerializeField] private float distanciaY = 0.2f;

    [ContextMenu("Organizar Hijos en Y")]

    public int RR(int i)
    {
      return  Random.Range(0, i);
    }

    public void OrganizarHijos()
    {
        Articulos = GameManager.instance.CajasDonaciones[GameManager.instance.CajaAUsar];

            for (int i = 0; i < Spawns.Count; i++)
            {
                GameObject objeto = Articulos[i];
                objeto.SetActive(true);
                objeto.transform.position = new Vector3 (transform.position.x,Spawns[i].transform.position.y,Spawns[i].transform.position.z);
                objeto.transform.SetParent(Spawns[i].transform);
            }

        GameManager.instance.CajaAUsar++;        
        
    }

    public int ContarObjetosPuestos()
    {
        int objetos = 0;
        foreach(GameObject child in Spawns)
        {
            if(child.transform.childCount>0)
            {
                objetos++;
            }
            
        }

        return objetos;
    }
}