using UnityEngine;
using Enums;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class DrawbridgeCustomizer : MonoBehaviour
{

    public ColorEnum drawbridgeColor;
    [HideInInspector][SerializeField] private int colorIndex = -1;
    public DrawbridgeController drawbridgeController;
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

    public void changeDrawbridgeColor(ColorEnum newColor)
    {
        switch (newColor)
        {
            case ColorEnum.Yellow:
                drawbridgeController.changeDrawbridgeColor(yellowColor, yellowRailingColor);
                break;

            case ColorEnum.Blue:
                drawbridgeController.changeDrawbridgeColor(blueColor, blueRailingColor);
                break;

            case ColorEnum.Green:
                drawbridgeController.changeDrawbridgeColor(greenColor, greenRailingColor);
                break;

            case ColorEnum.Pink:
                drawbridgeController.changeDrawbridgeColor(pinkColor, pinkRailingColor);
                break;
        }

        drawbridgeColor = newColor;

        // Save new color
        drawbridgeColor = newColor;

        // Set platform name
        colorIndex = -1;
        setName(newColor);

        // Needs to be here in order to save changes made in editor
        EditorUtility.SetDirty(drawbridgeController);
    }

    private void setName(ColorEnum newColor)
    {
        if (colorIndex == -1)
        {
            // Find all PlatformCustomizers in the scene (including inactives)
            DrawbridgeCustomizer[] allDrawbridges = FindObjectsByType<DrawbridgeCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            // Creates a list and populates it with all the platforms that already exist with that color
            List<DrawbridgeCustomizer> coloredDrawbridges = new List<DrawbridgeCustomizer>(50);


            // Fill newly created list with null entries
            while (coloredDrawbridges.Count < coloredDrawbridges.Capacity)
            {
                coloredDrawbridges.Add(null);
            }


            foreach (DrawbridgeCustomizer drawbridge in allDrawbridges)
            {
                if (drawbridge.drawbridgeColor == newColor && drawbridge.colorIndex != -1)
                {
                    coloredDrawbridges[drawbridge.colorIndex] = drawbridge;
                }
            }

            // Finds first null position in the list and inserts the new platform
            int firstNullSpace = coloredDrawbridges.FindIndex(item => item == null);

            if (firstNullSpace == -1)
            {
                firstNullSpace = coloredDrawbridges.Count;
            }

            // Assigns that value to colorIndex
            colorIndex = firstNullSpace;

        }

        // Sets object name according to drawbridge color and amount of drawbridges already with that color
        gameObject.name = drawbridgeColor.ToString() + " Drawbridge " + (colorIndex + 1);

    }

    private bool isThereObjectInSceneWithSameName()
    {
        bool result = false;

        DrawbridgeCustomizer[] allDrawbridges = FindObjectsByType<DrawbridgeCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (DrawbridgeCustomizer drawbridge in allDrawbridges)
        {
            if (drawbridge.drawbridgeColor == drawbridgeColor && drawbridge.gameObject.name.Equals(gameObject.name))
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
                setName(drawbridgeColor);
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
        if (drawbridgeController != null)
        {
            changeDrawbridgeColor(drawbridgeColor);
        }
    }

    /*
    *   Get / Set Functions
    */
    public void setRotationAmount(float amount)
    {
        rotationAmount = amount;
        
        drawbridgeController.setRotationAmount(rotationAmount);
        EditorUtility.SetDirty(drawbridgeController);

    }
    public void setRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
        drawbridgeController.setRotationSpeed(rotationSpeed);
        EditorUtility.SetDirty(drawbridgeController);
    }  

#endif
}


/******************************************************************************
*       EDITOR SUPPORT CLASS FOR DRAWBRIDGE CUSTOMIZER
*******************************************************************************/

#if UNITY_EDITOR
[CustomEditor(typeof(DrawbridgeCustomizer))]
public class DrawbridgeEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var customizer = (DrawbridgeCustomizer)target;

        EditorGUI.BeginChangeCheck();
        ColorEnum newColor = (ColorEnum)EditorGUILayout.EnumPopup("Drawbridge Color", customizer.drawbridgeColor);
        float newRotationAmount = EditorGUILayout.FloatField("Rotation Amount", customizer.rotationAmount);
        float newRotationSpeed = EditorGUILayout.FloatField("Rotation Speed", customizer.rotationSpeed);        

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customizer, "Changed Teleporter Parameters");

            customizer.changeDrawbridgeColor(newColor);
            customizer.setRotationAmount(newRotationAmount);
            customizer.setRotationSpeed(newRotationSpeed);            

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}
#endif
