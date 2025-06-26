using UnityEngine;
using Enums;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using System.Text.RegularExpressions;




#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PlatformCustomizer : MonoBehaviour
{

    [HideInInspector] public ColorEnum platformColor;
    [HideInInspector][SerializeField] private int colorIndex = -1;
    [HideInInspector] public Obstacle interactsWith;
    [HideInInspector] public InteractibleArea interactibleArea;
    [HideInInspector] public List<Obstacle> interactsWithList = new List<Obstacle>();

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

    public void doInteraction()
    {
        //interactsWith.interactWith();


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
                interactibleArea.changePlatformColor(yellowPlatformSprite, yellowWireMaterial);
                break;

            case ColorEnum.Blue:
                interactibleArea.changePlatformColor(bluePlatformSprite, blueWireMaterial);
                break;

            case ColorEnum.Green:
                interactibleArea.changePlatformColor(greenPlatformSprite, greenWireMaterial);
                break;


            case ColorEnum.Pink:
                interactibleArea.changePlatformColor(pinkPlatformSprite, pinkWireMaterial);
                break;
        }

        // Save new color
        platformColor = newColor;

        // Set platform name
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

            Debug.Log("First Null Space: " + firstNullSpace);

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



    public void Awake()
    {
        // Needs to be here for updating reasons; updates object from the old "interactsWith" variable to the new "interactsWithList" variable
        setObstacle(null);

        // Only runs setName if the object's name doesn't have a number in it yet OR if it contains a "(integer)" in its string
        if (!gameObject.name.Any(char.IsDigit) || Regex.IsMatch(gameObject.name, @"\(\d+\)"))
        {
            colorIndex = -1;
            setName(platformColor);
        }
    }


    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (!gameObject.name.Any(char.IsDigit) || Regex.IsMatch(gameObject.name, @"\(\d+\)"))
            {
                colorIndex = -1;
                setName(platformColor);
            }
        }
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

        // Interacted Object (DEPRECATED)
        // Obstacle newObstacle = (Obstacle)EditorGUILayout.ObjectField("Interacts With", customizer.interactsWith, typeof(Obstacle), true);

        // Interacted Obstacle List
        SerializedObject so = new SerializedObject(customizer);
        SerializedProperty obstacleList = so.FindProperty("interactsWithList");

        EditorGUILayout.PropertyField(obstacleList, new GUIContent("Interacts With Obstacles"), true);

        so.ApplyModifiedProperties();        

        // Record Changes
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customizer, "Changed Platform Settings");

            customizer.changePlatformColor(newColor);
            //customizer.setObstacle(newObstacle);

            // Visually update color selected in the dropdown menu
            customizer.platformColor = newColor;

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}

#endif

