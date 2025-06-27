using UnityEngine;
using Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PlatformCustomizer : MonoBehaviour
{

    [HideInInspector] public ColorEnum platformColor;
    [HideInInspector] public bool isWireEmissive;
    [HideInInspector] public bool wireStaysOn;
    [HideInInspector][SerializeField] private int colorIndex = -1;
    [HideInInspector] public Obstacle interactsWith;
    [HideInInspector] public InteractibleArea interactibleArea;
    [HideInInspector] public List<Obstacle> interactsWithList = new List<Obstacle>();

    // Colored Platform Materials, only touch if they need changing

    // Default Pressed Wire Material
    public Material redWireMaterial;

    // Yellow Platform
    public Sprite yellowPlatformSprite;
    public Material yellowWireMaterial;
    public Material yellowEmissiveWireMaterial;

    // Green Platform
    public Sprite greenPlatformSprite;
    public Material greenWireMaterial;
    public Material greenEmissiveWireMaterial;

    // Blue Platform
    public Sprite bluePlatformSprite;
    public Material blueWireMaterial;
    public Material blueEmissiveWireMaterial;

    // Pink Platform
    public Sprite pinkPlatformSprite;
    public Material pinkWireMaterial;
    public Material pinkEmissiveWireMaterial;

    public void doInteraction()
    {
        foreach (var obstacle in interactsWithList)
        {
            if (obstacle != null)
            {
                obstacle.interactWith();
            }
        }
    }

#if UNITY_EDITOR
    public void changePlatformColor(ColorEnum newColor)
    {
        // Change the onject's sprite and wire's material
        switch (newColor)
        {
            case ColorEnum.Yellow:
                if (isWireEmissive)
                {
                    interactibleArea.changePlatformColor(yellowPlatformSprite, yellowWireMaterial, yellowEmissiveWireMaterial);
                }
                else interactibleArea.changePlatformColor(yellowPlatformSprite, yellowWireMaterial, redWireMaterial);
                break;

            case ColorEnum.Blue:
                if (isWireEmissive)
                {
                    interactibleArea.changePlatformColor(bluePlatformSprite, blueWireMaterial, blueEmissiveWireMaterial);
                }
                else interactibleArea.changePlatformColor(bluePlatformSprite, blueWireMaterial, redWireMaterial);
                break;

            case ColorEnum.Green:
                if (isWireEmissive)
                {
                    interactibleArea.changePlatformColor(greenPlatformSprite, greenWireMaterial, greenEmissiveWireMaterial);
                }
                else interactibleArea.changePlatformColor(greenPlatformSprite, greenWireMaterial, redWireMaterial);
                break;


            case ColorEnum.Pink:
                if (isWireEmissive)
                {
                    interactibleArea.changePlatformColor(pinkPlatformSprite, pinkWireMaterial, pinkEmissiveWireMaterial);
                }
                else interactibleArea.changePlatformColor(pinkPlatformSprite, pinkWireMaterial, redWireMaterial);
                break;
        }

        // Save new color
        platformColor = newColor;

        // Set platform name
        colorIndex = -1;
        setName(newColor);

        // Needs to be here in order to save changes made in editor
        EditorUtility.SetDirty(interactibleArea);
    }

    private void setName(ColorEnum newColor)
    {
        if (colorIndex == -1)
        {
            // Find all PlatformCustomizers in the scene (including inactives)
            PlatformCustomizer[] allPlatforms = FindObjectsByType<PlatformCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            // Creates a list and populates it with all the platforms that already exist with that color
            List<PlatformCustomizer> coloredPlatforms = new List<PlatformCustomizer>(50);


            // Fill newly created list with null entries
            while (coloredPlatforms.Count < coloredPlatforms.Capacity)
            {
                coloredPlatforms.Add(null);
            }


            foreach (PlatformCustomizer platform in allPlatforms)
            {
                if (platform.platformColor == newColor && platform.colorIndex != -1)
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
        gameObject.name = platformColor.ToString() + " Interactive Platform " + (colorIndex + 1);

    }

    public void setObstacle(Obstacle obstacle)
    {
        //interactsWith = obstacle;

        if (!interactsWithList.Contains(interactsWith) && interactsWith != null)
        {
            interactsWithList.Add(interactsWith);
        }

        interactsWith = null;
    }

    private bool isThereObjectInSceneWithSameName()
    {
        bool result = false;

        PlatformCustomizer[] allPlatforms = FindObjectsByType<PlatformCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (PlatformCustomizer platform in allPlatforms)
        {
            if (platform.platformColor == platformColor && platform.gameObject.name.Equals(gameObject.name))
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
                setName(platformColor);
            }
        }
    }

    public void Awake()
    {
        // Needs to be here for updating reasons; updates object from the old "interactsWith" variable to the new "interactsWithList" variable
        setObstacle(null);
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
        if (interactibleArea != null)
        {
            changePlatformColor(platformColor);
        }
    }
    
    /*
    *   Set Functions
    */
    public void setIsWireEmissive(bool newIsWireEmissive)
    {
        isWireEmissive = newIsWireEmissive;
    }
    public void setWireStaysOn(bool newWireStaysOn)
    {
        wireStaysOn = newWireStaysOn;
        interactibleArea.setWireStaysOn(wireStaysOn);
        EditorUtility.SetDirty(interactibleArea);
    } 
#endif
}


/******************************************************************************
*       EDITOR SUPPORT CLASS FOR PLATFORM CUSTOMIZER
*******************************************************************************/

#if UNITY_EDITOR
[CustomEditor(typeof(PlatformCustomizer))]
public class PlatformEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var customizer = (PlatformCustomizer)target;

        // Start Changes
        EditorGUI.BeginChangeCheck();

        // Platform Color
        ColorEnum newColor = (ColorEnum)EditorGUILayout.EnumPopup("Platform Color", customizer.platformColor);

        // Interacted Obstacle List
        SerializedObject so = new SerializedObject(customizer);
        SerializedProperty obstacleList = so.FindProperty("interactsWithList");

        EditorGUILayout.PropertyField(obstacleList, new GUIContent("Interacts With Obstacles"), true);

        so.ApplyModifiedProperties();

        bool isWireEmissive = EditorGUILayout.Toggle("Emissive Wire", customizer.isWireEmissive);
        bool wireStaysOn = EditorGUILayout.Toggle("Wire Stays On", customizer.wireStaysOn);

        // Record Changes
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customizer, "Changed Platform Settings");

            customizer.setIsWireEmissive(isWireEmissive);
            customizer.setWireStaysOn(wireStaysOn);
            customizer.changePlatformColor(newColor);

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}

#endif

