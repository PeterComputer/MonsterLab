using UnityEngine;
using Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class KeypadPlatformCustomizer : MonoBehaviour
{

    [HideInInspector] public ColorEnum platformColor;
    [HideInInspector] public bool isWireEmissive;
    [HideInInspector] public bool hasWire;
    [HideInInspector] public bool wireStaysOn;
    [HideInInspector][SerializeField] private int colorIndex = -1;

    // The keypadArea component of this platform
    public KeypadAreaController keypadController;

    // The interactibleArea component of this platform
    [HideInInspector] public InteractibleArea interactibleArea;

    [HideInInspector] public bool interactsWithOtherObstacles;

    // The list of obstacles this object interacts with, if any
    [HideInInspector] public List<Obstacle> interactsWithList = new List<Obstacle>();

    // Colored Platform Materials, only touch if they need changing

    // Default Pressed Wire Material
    public Material redWireMaterial;

    // Default Pressed Platform Sprite
    public Sprite redPlatformSprite;


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

    // Gets called in an _onTriggerEnter event in the InteractibleArea inspector (hidden by default)
    public void doInteraction()
    {
        // Interact with the keypad component
        keypadController.interactWith();

        // Interact with other objects
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
                    interactibleArea.changePlatformColor(yellowPlatformSprite, redPlatformSprite, yellowWireMaterial, yellowEmissiveWireMaterial);
                }
                else interactibleArea.changePlatformColor(yellowPlatformSprite, redPlatformSprite, yellowWireMaterial, redWireMaterial);
                break;

            case ColorEnum.Blue:
                if (isWireEmissive)
                {
                    interactibleArea.changePlatformColor(bluePlatformSprite, redPlatformSprite, blueWireMaterial, blueEmissiveWireMaterial);
                }
                else interactibleArea.changePlatformColor(bluePlatformSprite, redPlatformSprite, blueWireMaterial, redWireMaterial);
                break;

            case ColorEnum.Green:
                if (isWireEmissive)
                {
                    interactibleArea.changePlatformColor(greenPlatformSprite, redPlatformSprite, greenWireMaterial, greenEmissiveWireMaterial);
                }
                else interactibleArea.changePlatformColor(greenPlatformSprite, redPlatformSprite, greenWireMaterial, redWireMaterial);
                break;


            case ColorEnum.Pink:
                if (isWireEmissive)
                {
                    interactibleArea.changePlatformColor(pinkPlatformSprite, redPlatformSprite, pinkWireMaterial, pinkEmissiveWireMaterial);
                }
                else interactibleArea.changePlatformColor(pinkPlatformSprite, redPlatformSprite, pinkWireMaterial, redWireMaterial);
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
            // Find all KeypadPlatformCustomizers in the scene (including inactives)
            KeypadPlatformCustomizer[] allPlatforms = FindObjectsByType<KeypadPlatformCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            // Creates a list and populates it with all the platforms that already exist with that color
            List<KeypadPlatformCustomizer> coloredPlatforms = new List<KeypadPlatformCustomizer>(50);


            // Fill newly created list with null entries
            while (coloredPlatforms.Count < coloredPlatforms.Capacity)
            {
                coloredPlatforms.Add(null);
            }


            foreach (KeypadPlatformCustomizer platform in allPlatforms)
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
        gameObject.name = platformColor.ToString() + " Keypad Platform " + (colorIndex + 1);

    }

    private bool isThereObjectInSceneWithSameName()
    {
        bool result = false;

        KeypadPlatformCustomizer[] allPlatforms = FindObjectsByType<KeypadPlatformCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (KeypadPlatformCustomizer platform in allPlatforms)
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
        hasWire = interactibleArea.getHasWire();
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
    public void setHasWire(bool newHasWire)
    {
        hasWire = newHasWire;
        interactibleArea.setHasWire(hasWire);
    }
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

    public void setInteractsWithOtherObstacles(bool newInteractsWithOtherObstacles)
    {
        interactsWithOtherObstacles = newInteractsWithOtherObstacles;
    }
#endif
}


/******************************************************************************
*       EDITOR SUPPORT CLASS FOR PLATFORM CUSTOMIZER
*******************************************************************************/

#if UNITY_EDITOR
[CustomEditor(typeof(KeypadPlatformCustomizer))]
public class KeypadPlatformEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var customizer = (KeypadPlatformCustomizer)target;

        // Start Changes
        EditorGUI.BeginChangeCheck();

        // Platform Color
        ColorEnum newColor = (ColorEnum)EditorGUILayout.EnumPopup("Platform Color", customizer.platformColor);

        bool interactsWithOtherObstacles = EditorGUILayout.Toggle("Interacts With Other Obstacles?", customizer.interactsWithOtherObstacles);

        if (interactsWithOtherObstacles)
        {
            // Interacted Obstacle List
            SerializedObject so = new SerializedObject(customizer);
            SerializedProperty obstacleList = so.FindProperty("interactsWithList");

            EditorGUILayout.PropertyField(obstacleList, new GUIContent("Interacts With Obstacles"), true);

            so.ApplyModifiedProperties();            
        }

        bool isWireEmissive;
        bool wireStaysOn;

        bool hasWire = EditorGUILayout.Toggle("Has Wire", customizer.hasWire);

        // Display the "isWireEmissive" and "WireStaysOn" fields only if the platform has a wire
        if (hasWire)
        {
            isWireEmissive = EditorGUILayout.Toggle("Emissive Wire", customizer.isWireEmissive);
            wireStaysOn = EditorGUILayout.Toggle("Wire Stays On", customizer.wireStaysOn);
        }
        else
        {
            isWireEmissive = customizer.isWireEmissive;
            wireStaysOn = customizer.wireStaysOn;
        }


        // Record Changes
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customizer, "Changed Platform Settings");

            customizer.setHasWire(hasWire);
            customizer.setInteractsWithOtherObstacles(interactsWithOtherObstacles);  

            if (hasWire)
            {
                customizer.setIsWireEmissive(isWireEmissive);
                customizer.setWireStaysOn(wireStaysOn);
            }

            customizer.changePlatformColor(newColor);

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}

#endif