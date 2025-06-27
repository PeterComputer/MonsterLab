 using UnityEngine;
using Enums;
using PathCreation.Examples;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;




#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class FenceGateCustomizer : MonoBehaviour
{
    [HideInInspector] public ColorEnum fenceGateColor;
    [SerializeField] private SpriteRenderer fenceGateSpriteRenderer;
    [HideInInspector][SerializeField] private int colorIndex = -1;
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

#if UNITY_EDITOR
    public void changeFenceGateColor(ColorEnum newColor)
    {
        switch (newColor)
        {
            case ColorEnum.Yellow:

                fenceGateSpriteRenderer.sprite = yellowFenceGateSprite;
                wire.roadMaterial = yellowWireMaterial;
                wire.TriggerUpdate();
                break;

            case ColorEnum.Blue:
                fenceGateSpriteRenderer.sprite = blueFenceGateSprite;
                wire.roadMaterial = blueWireMaterial;
                wire.TriggerUpdate();
                break;

            case ColorEnum.Green:
                fenceGateSpriteRenderer.sprite = greenFenceGateSprite;
                wire.roadMaterial = greenWireMaterial;
                wire.TriggerUpdate();
                break;


            case ColorEnum.Pink:
                fenceGateSpriteRenderer.sprite = pinkFenceGateSprite;
                wire.roadMaterial = pinkWireMaterial;
                wire.TriggerUpdate();
                break;
        }

        // Save new color
        fenceGateColor = newColor;

        // Set platform name
        colorIndex = -1;
        setName(newColor);
    }

    private void setName(ColorEnum newColor)
    {
        if (colorIndex == -1)
        {
            // Find all FenceGateCustomizers in the scene (including inactives)
            FenceGateCustomizer[] allPlatforms = FindObjectsByType<FenceGateCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            // Creates a list and populates it with all the platforms that already exist with that color
            List<FenceGateCustomizer> coloredPlatforms = new List<FenceGateCustomizer>(50);


            // Fill newly created list with null entries
            while (coloredPlatforms.Count < coloredPlatforms.Capacity)
            {
                coloredPlatforms.Add(null);
            }


            foreach (FenceGateCustomizer platform in allPlatforms)
            {
                if (platform.fenceGateColor == newColor && platform.colorIndex != -1)
                {


                    coloredPlatforms[platform.colorIndex] = platform;
                }
            }

            // Finds first null position in the list and inserts the new platform
            int firstNullSpace = coloredPlatforms.FindIndex(item => item == null);

            if (firstNullSpace == -1)
            {
                firstNullSpace = coloredPlatforms.Count;
            }

            // Assigns that value to colorIndex
            colorIndex = firstNullSpace;

        }

        // Sets object name according to platform color and amount of platforms already with that color
        gameObject.name = fenceGateColor.ToString() + " Fence Gate Frame " + (colorIndex + 1);

        // Sets the name of the actual Fence Gate inside the Fence Gate Frame
        gameObject.GetComponentInChildren<MovingGateController>().gameObject.name = fenceGateColor.ToString() + " Fence Gate " + (colorIndex + 1);

    }

    private bool isThereObjectInSceneWithSameName()
    {
        bool result = false;

        FenceGateCustomizer[] allPlatforms = FindObjectsByType<FenceGateCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (FenceGateCustomizer platform in allPlatforms)
        {
            if (platform.fenceGateColor == fenceGateColor && platform.gameObject.name.Equals(gameObject.name))
            {
                result = true;
            }
        }
        return result;
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            // Only runs setName if the object's name doesn't have a number in it yet (present here for updating reasons)
            // OR if it contains a "(integer)" in its string (present here for updating reasons)
            // OR there's already an object in the scene with that same name
            if (!gameObject.name.Any(char.IsDigit) || Regex.IsMatch(gameObject.name, @"\(\d+\)") || isThereObjectInSceneWithSameName())
            {
                colorIndex = -1;
                setName(fenceGateColor);
            }
        }
    }

    public void OnEnable()
    {
        // Support for undoing actions. Not the most efficient, since its running on ALL undo events, not just the ones related to this object
        Undo.undoRedoPerformed += OnUndoRedo;
    }
    public void OnDestroy()
    {
        Undo.undoRedoPerformed -= OnUndoRedo;
    }
    private void OnUndoRedo()
    {
        if (fenceGateSpriteRenderer != null)
        {
            changeFenceGateColor(fenceGateColor);            
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(FenceGateCustomizer))]
public class FenceGateEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var customizer = (FenceGateCustomizer)target;

        EditorGUI.BeginChangeCheck();
        ColorEnum newColor = (ColorEnum)EditorGUILayout.EnumPopup("Fence Gate Color", customizer.fenceGateColor);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customizer, "Changed Fence Gate Color");

            customizer.changeFenceGateColor(newColor);

            // Visually update color selected in the dropdown menu
            customizer.fenceGateColor = newColor;

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}
#endif