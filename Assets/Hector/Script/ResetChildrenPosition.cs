using JetBrains.Annotations;
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

        for (int i = 0; i < Spawns.Count; i++)
        {
            GameObject objeto = Instantiate(Articulos[RR(Articulos.Count)], new Vector3(0f, 0, 0), Quaternion.identity);
            //  objeto.transform.SetParent(mesa.transform);
            objeto.transform.position = Spawns[i].transform.position;
        }
        
    }

    private void Start()
    {
        // Opcional: Descomenta esta línea si quieres que lo haga automáticamente al iniciar el juego.
   
    }
}