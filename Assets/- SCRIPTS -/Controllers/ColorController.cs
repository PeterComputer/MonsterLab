using UnityEngine;

public class ColorController : MonoBehaviour
{
    MaterialPropertyBlock propertyBlock;

    public float hueOffset;
    public float saturationValue;
    public float tilingMultiplier;
    public bool isColorReplacementActive;

    void OnValidate()
    {

        if(propertyBlock == null) {
            propertyBlock = new MaterialPropertyBlock();
        }

        Renderer renderer = GetComponent<Renderer>();

        if (isColorReplacementActive) {

            propertyBlock.SetFloat("_HueOffset", hueOffset);
            propertyBlock.SetFloat("_SaturationValue", saturationValue);
            propertyBlock.SetFloat("_TilingMultiplier", tilingMultiplier);
        }   
            
        else {
            propertyBlock.SetFloat("_HueOffset", 0f);
            propertyBlock.SetFloat("_SaturationValue", 1f);
            propertyBlock.SetFloat("_TilingMultiplier", 1f);
        }

        renderer.SetPropertyBlock(propertyBlock, 0);

    }

    void Awake() {
        OnValidate();
    }
        

}
