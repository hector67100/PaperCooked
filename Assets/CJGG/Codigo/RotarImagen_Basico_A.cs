using UnityEngine;

public class RotarImagen_Basico_A : MonoBehaviour
{
    public Vector3 XYZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public float CalcularLimites(float X,float x)
    {
        X = X + x;
        if (X < -360)
            X = X + 360;
        if (X > 360)
            X = X - 360;
        return X;
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localEulerAngles = new Vector3(CalcularLimites(gameObject.transform.localEulerAngles.x, XYZ.x), CalcularLimites(gameObject.transform.localEulerAngles.y, XYZ.y), CalcularLimites(gameObject.transform.localEulerAngles.z, XYZ.z)); 
    }
}
