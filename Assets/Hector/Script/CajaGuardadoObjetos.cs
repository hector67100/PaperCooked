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

        int check = ContarObjetosPuestos();
        Debug.Log(check);
        if(check == spawns.Length)
        {
            return;    
        }

        Debug.Log(check);

        if(tieneObjetos)
        {

         int index = check;
         int borrar = 0;   
            foreach(GameObject child in objetosAGuardar)
            {
                if (index == spawns.Length) break;
                int posicion = ConseguirSpawnLibre();
                Debug.Log(posicion);
                child.SetActive(true);
                child.transform.SetParent(spawns[posicion]);
                child.transform.position = spawns[posicion].position;
                index++;
                borrar++;
            }

            objetosAGuardar.RemoveRange(0,borrar);
            
            if(objetosAGuardar.Count == 0)
            {
                tieneObjetos = false;
            }
        }
    }

    public int ContarObjetosPuestos()
    {
        int objetos = 0;
        foreach(Transform child in Mesa.transform)
        {
            if(child.transform.tag != "Guardar" && child.transform.tag != "Mantel")
            {
                if(child.transform.childCount>0)
                {
                    objetos++;
                }
            }
        }

        return objetos;
    }

    public int ConseguirSpawnLibre()
    {
        int spawnVacio = 0;
        foreach(Transform child in Mesa.transform)
        {
            Debug.Log(child.name);
            if( child.transform.tag != "Guardar" && child.transform.tag != "Mantel")
            {
                
                if(child.transform.childCount==0)
                {
                    return spawnVacio;
                }

                spawnVacio++;
            }
        }

        return spawnVacio;
    }
}
