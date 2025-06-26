using UnityEngine;
using Enums;
using PathCreation.Examples;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FenceGateCustomizer : MonoBehaviour
{
    [HideInInspector] public ColorEnum fenceGateColor;
    [SerializeField] private SpriteRenderer fenceGateSpriteRenderer;
    [SerializeField] private RoadMeshCreator wire;

    // Colored Fence Gate Materials, only touch if they need changing

    // Yellow Fence Gate
    public Sprite yellowFenceGateSprite;
    public Material yellowWireMaterial;

    // Green Fence Gate
    public Sprite greenFenceGateSprite;
    public Material greenWireMaterial;

    // Blue Fence Gate
    public Sprite blueFenceGateSprite;
    public Material blueWireMaterial;

    // Pink Fence Gate
    public Sprite pinkFenceGateSprite;
    public Material pinkWireMaterial;

    // Default Fence Gate
    public Sprite defaultFenceGateSprite;
    public Material defaultWireMaterial;

    public void changeFenceGateColor(ColorEnum newColor)
    {
        switch (newColor)
        {
            case ColorEnum.Yellow:

                fenceGateSpriteRenderer.sprite = yellowFenceGateSprite;
                wire.roadMaterial = yellowWireMaterial;
                wire.TriggerUpdate();
                gameObject.name = "Yellow Fence Gate Frame";
                gameObject.GetComponentInChildren<MovingGateController>().gameObject.name = "Yellow Fence Gate";
                break;

            case ColorEnum.Blue:
                fenceGateSpriteRenderer.sprite = blueFenceGateSprite;
                wire.roadMaterial = blueWireMaterial;
                wire.TriggerUpdate();
                gameObject.name = "Blue Fence Gate Frame";
                gameObject.GetComponentInChildren<MovingGateController>().gameObject.name = "Blue Fence Gate";
                break;

            case ColorEnum.Green:
                fenceGateSpriteRenderer.sprite = greenFenceGateSprite;
                wire.roadMaterial = greenWireMaterial;
                wire.TriggerUpdate();
                gameObject.name = "Green Fence Gate Frame";
                gameObject.GetComponentInChildren<MovingGateController>().gameObject.name = "Green Fence Gate";
                break;


            case ColorEnum.Pink:
                fenceGateSpriteRenderer.sprite = pinkFenceGateSprite;
                wire.roadMaterial = pinkWireMaterial;
                wire.TriggerUpdate();
                gameObject.name = "Pink Fence Gate Frame";
                gameObject.GetComponentInChildren<MovingGateController>().gameObject.name = "Pink Fence Gate";
                break;

                /*             case ColorEnum.None:
                                fenceGateSpriteRenderer.sprite = defaultFenceGateSprite;
                                wire.roadMaterial = defaultWireMaterial;
                                wire.TriggerUpdate();
                                gameObject.name = "Fence Gate Frame";
                                break; */
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FenceGateCustomizer))]
public class FenceGateEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var customizer = (FenceGateCustomizer)target;

        EditorGUI.BeginChangeCheck();
        ColorEnum newColor = (ColorEnum)EditorGUILayout.EnumPopup("Platform Color", customizer.fenceGateColor);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customizer, "Changed Platform Color");

            customizer.changeFenceGateColor(newColor);

            // Visually update color selected in the dropdown menu
            customizer.fenceGateColor = newColor;

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}
#endif