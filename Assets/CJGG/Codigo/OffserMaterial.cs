using UnityEngine;

public class OffserMaterial : MonoBehaviour
{
    public Vector2 Offset_custom;
    public Material rayos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rayos.mainTextureOffset = rayos.mainTextureOffset + Offset_custom;
    }
}
