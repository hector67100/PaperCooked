using UnityEngine;

public class DonacionSelector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject selector;
    void Start()
    {
        Transform hijoBrillo = transform.Find("brillo");

        if (hijoBrillo != null)
        {
            selector = hijoBrillo.gameObject;
            selector.SetActive(false);
        }
    }

    // Update is called once per frame

    public void Apagar()
    {
        selector.SetActive(false);
    }

    public void Activar()
    {
        selector.SetActive(true);
    }

}
