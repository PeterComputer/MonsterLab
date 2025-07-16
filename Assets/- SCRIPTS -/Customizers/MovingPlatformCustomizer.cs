using UnityEngine;
using Enums;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
public class MovingPlatformCustomizer : MonoBehaviour
{

    public ColorEnum movingPlatformColor;
    [HideInInspector][SerializeField] private int colorIndex = -1;
    public MovingPlatformController movingPlatformController;
    [SerializeField] public List<Transform> platformPositions;
    [HideInInspector] public float rotationAmount;
    [HideInInspector] public float rotationSpeed;

    // Color variables, only touch if they need changing

    public Material yellowColor;
    public Material yellowRailingColor;
    public Material blueColor;
    public Material blueRailingColor;
    public Material greenColor;
    public Material greenRailingColor;
    public Material pinkColor;
    public Material pinkRailingColor;


#if UNITY_EDITOR

    public void changeMovingPlatformColor(ColorEnum newColor)
    {
        switch (newColor)
        {
            case ColorEnum.Yellow:
                movingPlatformController.changeMovingPlatformColor(yellowColor, yellowRailingColor);
                break;

            case ColorEnum.Blue:
                movingPlatformController.changeMovingPlatformColor(blueColor, blueRailingColor);
                break;

            case ColorEnum.Green:
                movingPlatformController.changeMovingPlatformColor(greenColor, greenRailingColor);
                break;

            case ColorEnum.Pink:
                movingPlatformController.changeMovingPlatformColor(pinkColor, pinkRailingColor);
                break;
        }

        movingPlatformColor = newColor;

        // Save new color
        movingPlatformColor = newColor;

        // Set platform name
        colorIndex = -1;
        setName(newColor);

        // Needs to be here in order to save changes made in editor
        EditorUtility.SetDirty(movingPlatformController);
    }

    private void setName(ColorEnum newColor)
    {
        if (colorIndex == -1)
        {
            // Find all PlatformCustomizers in the scene (including inactives)
            MovingPlatformCustomizer[] allMovingPlatforms = FindObjectsByType<MovingPlatformCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            // Creates a list and populates it with all the platforms that already exist with that color
            List<MovingPlatformCustomizer> coloredMovingPlatforms = new List<MovingPlatformCustomizer>(50);


            // Fill newly created list with null entries
            while (coloredMovingPlatforms.Count < coloredMovingPlatforms.Capacity)
            {
                coloredMovingPlatforms.Add(null);
            }


            foreach (MovingPlatformCustomizer movingPlatform in allMovingPlatforms)
            {
                if (movingPlatform.movingPlatformColor == newColor && movingPlatform.colorIndex != -1)
                {
                    coloredMovingPlatforms[movingPlatform.colorIndex] = movingPlatform;
                }
            }

            // Finds first null position in the list and inserts the new platform
            int firstNullSpace = coloredMovingPlatforms.FindIndex(item => item == null);

            if (firstNullSpace == -1)
            {
                firstNullSpace = coloredMovingPlatforms.Count;
            }

            // Assigns that value to colorIndex
            colorIndex = firstNullSpace;

        }

        // Sets object name according to movingPlatform color and amount of movingPlatforms already with that color
        gameObject.name = movingPlatformColor.ToString() + " Moving Platform " + (colorIndex + 1);

    }

    private bool isThereObjectInSceneWithSameName()
    {
        bool result = false;

        MovingPlatformCustomizer[] allMovingPlatforms = FindObjectsByType<MovingPlatformCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (MovingPlatformCustomizer movingPlatform in allMovingPlatforms)
        {
            if (movingPlatform.movingPlatformColor == movingPlatformColor && movingPlatform.gameObject.name.Equals(gameObject.name))
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
                setName(movingPlatformColor);
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
        if (movingPlatformController != null)
        {
            changeMovingPlatformColor(movingPlatformColor);
        }
    }

    /*
    *   Get / Set Functions
    */
    public void setPlatformPositions()
    {
        movingPlatformController.setPlatformPositions(platformPositions);
        EditorUtility.SetDirty(movingPlatformController);
    }

#endif
}

/******************************************************************************
*       EDITOR SUPPORT CLASS FOR DRAWBRIDGE CUSTOMIZER
*******************************************************************************/

#if UNITY_EDITOR
[CustomEditor(typeof(MovingPlatformCustomizer))]
public class MovingPlatformEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var customizer = (MovingPlatformCustomizer)target;

        EditorGUI.BeginChangeCheck();
        ColorEnum newColor = (ColorEnum)EditorGUILayout.EnumPopup("Moving Platform Color", customizer.movingPlatformColor);

        // Interacted Obstacle List
        SerializedObject so = new SerializedObject(customizer);
        SerializedProperty obstacleList = so.FindProperty("platformPositions");

        EditorGUILayout.PropertyField(obstacleList, new GUIContent("Platform Positions"), true);

        so.ApplyModifiedProperties();          

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customizer, "Changed Moving Platform Parameters");

            customizer.changeMovingPlatformColor(newColor);
            customizer.setPlatformPositions();

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}
#endif