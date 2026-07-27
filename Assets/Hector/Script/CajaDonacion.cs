using UnityEngine;

public class CajaDonacion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool open = false;

    public GameObject[] donacionesCaja;

    public void AbrirCaja()
    {
        GameManager.instance.aparecerDonaciones(donacionesCaja);
        open = true;
    }
}
