using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;

public class CajaGuardadoObjetos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> objetosAGuardar = new List<GameObject>();
    public GameObject Mesa; 
    public bool tieneObjetos = false;
    [SerializeField] Transform[] spawns;

    void Start()
    {
        List<Transform> lista = new List<Transform>();
        foreach(Transform child in Mesa.transform)
        {
            if(child.tag =="Spawn")
            {
                lista.Add(child);
            }
        }
        spawns = lista.ToArray();
    }

    public void GuardarEnCaja(GameObject gameObject)
    {
        tieneObjetos = true;
        objetosAGuardar.Add(gameObject);
    }

    public void  SpawnearObjetosEnMesa()
    {
        // foreach
        int check = 0;
        foreach(Transform child in Mesa.transform)
        {
            if(child.transform.tag != "Spawn" || child.transform.tag != "Guardar")
            {
                check++;
            }
        }

        if(check == spawns.Length)
        {
            return;    
        }

        if(tieneObjetos)
        {
            int index = 0;
            foreach(GameObject child in objetosAGuardar)
            {
                if (index == spawns.Length) break;
                child.SetActive(true);
                child.transform.SetParent(Mesa.transform);
                child.transform.position = spawns[index].position;
                index++;
            }

            objetosAGuardar.RemoveRange(0,index);
            
            if(objetosAGuardar.Count == 0)
            {
                tieneObjetos = false;
            }
        }
    }
}
