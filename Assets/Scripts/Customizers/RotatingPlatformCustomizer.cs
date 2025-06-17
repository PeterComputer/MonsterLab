using UnityEngine;
using UnityEditor;
using Enums;
using System.Collections.Generic;
using System.Linq;

public class RotatingPlatformCustomizer : MonoBehaviour
{
    [HideInInspector] public ColorEnum rotatingPlatformColor;
    [SerializeField] private SpriteRenderer platformSpriteRenderer;
    [SerializeField] private List<SpriteRenderer> fenceGateSpriteRenderers;

    // Colored Platform Materials, only touch if they need changing

    // Yellow Platform
    public Sprite yellowPlatformSprite;
    public Sprite yellowFenceSprite;

    // Green Platform
    public Sprite greenPlatformSprite;
    public Sprite greenFenceSprite;

    // Blue Platform
    public Sprite bluePlatformSprite;
    public Sprite blueFenceSprite;

    // Pink Platform
    public Sprite pinkPlatformSprite;
    public Sprite pinkFenceSprite;


    public void changePlatformColor(ColorEnum newColor)
    {

        setChildrenFenceGates();

        switch (newColor)
        {
            case ColorEnum.Yellow:
                platformSpriteRenderer.sprite = yellowPlatformSprite;

                foreach (SpriteRenderer fenceGateRenderer in fenceGateSpriteRenderers)
                {
                    fenceGateRenderer.sprite = yellowFenceSprite;
                }

                gameObject.name = "Yellow Rotating Platform";
                break;

            case ColorEnum.Blue:
                platformSpriteRenderer.sprite = bluePlatformSprite;

                foreach (SpriteRenderer fenceGateRenderer in fenceGateSpriteRenderers)
                {
                    fenceGateRenderer.sprite = blueFenceSprite;
                }

                gameObject.name = "Blue Rotating Platform";
                break;

            case ColorEnum.Green:
                platformSpriteRenderer.sprite = greenPlatformSprite;

                foreach (SpriteRenderer fenceGateRenderer in fenceGateSpriteRenderers)
                {
                    fenceGateRenderer.sprite = greenFenceSprite;
                }

                gameObject.name = "Green Rotating Platform";
                break;


            case ColorEnum.Pink:
                platformSpriteRenderer.sprite = pinkPlatformSprite;

                foreach (SpriteRenderer fenceGateRenderer in fenceGateSpriteRenderers)
                {
                    fenceGateRenderer.sprite = pinkFenceSprite;
                }

                gameObject.name = "Pink Rotating Platform";
                break;
        }
    }

    private void setChildrenFenceGates()
    {
        fenceGateSpriteRenderers.Clear();
        List<SpriteRenderer> tempArray = gameObject.GetComponentsInChildren<SpriteRenderer>().ToList();

        foreach (SpriteRenderer temp in tempArray)
        {
            if (!temp.gameObject.Equals(platformSpriteRenderer.gameObject))
            {
                fenceGateSpriteRenderers.Add(temp);
            }
        }

    }
}

[CustomEditor(typeof(RotatingPlatformCustomizer))]
public class RotatingPlatformEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var customizer = (RotatingPlatformCustomizer)target;

        EditorGUI.BeginChangeCheck();
        ColorEnum newColor = (ColorEnum)EditorGUILayout.EnumPopup("Platform Color", customizer.rotatingPlatformColor);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customizer, "Changed Platform Color");

            customizer.changePlatformColor(newColor);

            // Visually update color selected in the dropdown menu
            customizer.rotatingPlatformColor = newColor;

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}