using UnityEngine;
using UnityEditor;
using Enums;

public class PlatformCustomizer : MonoBehaviour
{

    [HideInInspector] public ColorEnum platformColor;
    [HideInInspector] public ObstacleCustomizer interactsWith;

    [SerializeField] private InteractibleArea interactibleArea;


    // Colored Platform Materials, only touch if they need changing

    // Yellow Platform
    public Sprite yellowPlatformSprite;
    public Material yellowWireMaterial;

    // Green Platform
    public Sprite greenPlatformSprite;
    public Material greenWireMaterial;

    // Blue Platform
    public Sprite bluePlatformSprite;
    public Material blueWireMaterial;

    // Pink Platform
    public Sprite pinkPlatformSprite;
    public Material pinkWireMaterial;


    public void changePlatformColor(ColorEnum newColor)
    {
        switch (newColor)
        {
            case ColorEnum.Yellow:
                interactibleArea.changePlatformColor(yellowPlatformSprite, yellowWireMaterial);
                gameObject.name = "Yellow Interactive Platform";
                break;

            case ColorEnum.Blue:
                interactibleArea.changePlatformColor(bluePlatformSprite, blueWireMaterial);
                gameObject.name = "Blue Interactive Platform";
                break;

            case ColorEnum.Green:
                interactibleArea.changePlatformColor(greenPlatformSprite, greenWireMaterial);
                gameObject.name = "Green Interactive Platform";
                break;


            case ColorEnum.Pink:
                interactibleArea.changePlatformColor(pinkPlatformSprite, pinkWireMaterial);
                gameObject.name = "Pink Interactive Platform";
                break;
        }
    }
}


[CustomEditor(typeof(PlatformCustomizer))]
public class PlatformEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var customizer = (PlatformCustomizer)target;

        EditorGUI.BeginChangeCheck();
        ColorEnum newColor = (ColorEnum)EditorGUILayout.EnumPopup("Platform Color", customizer.platformColor);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customizer, "Changed Platform Color");

            customizer.changePlatformColor(newColor);

            // Visually update color selected in the dropdown menu
            customizer.platformColor = newColor;

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}


