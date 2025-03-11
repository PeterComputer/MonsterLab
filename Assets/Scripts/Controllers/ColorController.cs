using UnityEngine;

public class ColorController : MonoBehaviour
{
    MaterialPropertyBlock propertyBlock;
    public Color colorToReplace;
    public Color replacementColor;
    public float replacementRange;
    public float replacementFuzziness;

    public bool isColorReplacementActive;

    void OnValidate()
    {

        if(propertyBlock == null) {
            propertyBlock = new MaterialPropertyBlock();
        }

        Renderer renderer = GetComponent<Renderer>();

        if (isColorReplacementActive) {

            propertyBlock.SetColor("_Color_To_Replace", colorToReplace);
            propertyBlock.SetColor("_Replacement_Color", replacementColor);
            propertyBlock.SetFloat("_Replacement_Range", replacementRange);
            propertyBlock.SetFloat("_Replacement_Fuzziness", replacementFuzziness);
        }   
            
        else {
            propertyBlock.SetFloat("_Replacement_Range", 0f);
            propertyBlock.SetFloat("_Replacement_Fuzziness", 0f);    
        }

        renderer.SetPropertyBlock(propertyBlock);

    }
        

}
