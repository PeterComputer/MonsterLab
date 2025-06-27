using UnityEngine;
using Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;



#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class RotatingPlatformCustomizer : MonoBehaviour
{
    [HideInInspector] public ColorEnum rotatingPlatformColor;
    [HideInInspector][SerializeField] private int colorIndex = -1;
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

#if UNITY_EDITOR
    public void changeRotatingPlatformColor(ColorEnum newColor)
    {
        // Get the fence gates of the rotating platform, so they can be colored in the switch()
        setChildrenFenceGates();

        switch (newColor)
        {
            case ColorEnum.Yellow:
                platformSpriteRenderer.sprite = yellowPlatformSprite;

                foreach (SpriteRenderer fenceGateRenderer in fenceGateSpriteRenderers)
                {
                    fenceGateRenderer.sprite = yellowFenceSprite;
                }
                break;

            case ColorEnum.Blue:
                platformSpriteRenderer.sprite = bluePlatformSprite;

                foreach (SpriteRenderer fenceGateRenderer in fenceGateSpriteRenderers)
                {
                    fenceGateRenderer.sprite = blueFenceSprite;
                }
                break;

            case ColorEnum.Green:
                platformSpriteRenderer.sprite = greenPlatformSprite;

                foreach (SpriteRenderer fenceGateRenderer in fenceGateSpriteRenderers)
                {
                    fenceGateRenderer.sprite = greenFenceSprite;
                }
                break;

            case ColorEnum.Pink:
                platformSpriteRenderer.sprite = pinkPlatformSprite;

                foreach (SpriteRenderer fenceGateRenderer in fenceGateSpriteRenderers)
                {
                    fenceGateRenderer.sprite = pinkFenceSprite;
                }
                break;
        }

        // Save new color
        rotatingPlatformColor = newColor;

        // Set platform name
        colorIndex = -1;
        setName(newColor);
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

    private void setName(ColorEnum newColor)
    {
        if (colorIndex == -1)
        {
            // Find all RotatingPlatformCustomizers in the scene (including inactives)
            RotatingPlatformCustomizer[] allPlatforms = FindObjectsByType<RotatingPlatformCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            // Creates a list and populates it with all the platforms that already exist with that color
            List<RotatingPlatformCustomizer> coloredPlatforms = new List<RotatingPlatformCustomizer>(50);


            // Fill newly created list with null entries
            while (coloredPlatforms.Count < coloredPlatforms.Capacity)
            {
                coloredPlatforms.Add(null);
            }


            foreach (RotatingPlatformCustomizer platform in allPlatforms)
            {
                if (platform.rotatingPlatformColor == newColor && platform.colorIndex != -1)
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
        gameObject.name = rotatingPlatformColor.ToString() + " Rotating Platform " + (colorIndex + 1);

    }

    private bool isThereObjectInSceneWithSameName()
    {
        bool result = false;

        RotatingPlatformCustomizer[] allPlatforms = FindObjectsByType<RotatingPlatformCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (RotatingPlatformCustomizer platform in allPlatforms)
        {
            if (platform.rotatingPlatformColor == rotatingPlatformColor && platform.gameObject.name.Equals(gameObject.name))
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
                setName(rotatingPlatformColor);
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
        if (platformSpriteRenderer != null && fenceGateSpriteRenderers != null)
        {
            changeRotatingPlatformColor(rotatingPlatformColor);           
        }
    }    
#endif
}


/******************************************************************************
*       EDITOR SUPPORT CLASS FOR PLATFORM CUSTOMIZER
*******************************************************************************/

#if UNITY_EDITOR
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

            customizer.changeRotatingPlatformColor(newColor);

            // Visually update color selected in the dropdown menu
            customizer.rotatingPlatformColor = newColor;

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}
#endif