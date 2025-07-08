using UnityEngine;
using Enums;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

[ExecuteInEditMode]
public class TeleporterCustomizer : MonoBehaviour
{

    public ColorEnum teleporterColor;
    [HideInInspector][SerializeField] private int colorIndex = -1;
    [HideInInspector] public TeleporterCustomizer pairedTeleporterCustomizer;
    public TeleporterController teleporterController;

    // Color variables, only touch if they need changing

    public Color yellowColor;
    public Color yellowPortalColor;
    public Color blueColor;
    public Color bluePortalColor;
    public Color greenColor;
    public Color greenPortalColor;
    public Color pinkColor;
    public Color pinkPortalColor;


#if UNITY_EDITOR

    public void changeTeleporterColor(ColorEnum newColor)
    {
        switch (newColor)
        {
            case ColorEnum.Yellow:
                teleporterController.changeTeleporterColor(yellowColor, yellowPortalColor);
                break;

            case ColorEnum.Blue:
                teleporterController.changeTeleporterColor(blueColor, bluePortalColor);
                break;

            case ColorEnum.Green:
                teleporterController.changeTeleporterColor(greenColor, greenPortalColor);
                break;

            case ColorEnum.Pink:
                teleporterController.changeTeleporterColor(pinkColor, pinkPortalColor);
                break;
        }

        teleporterColor = newColor;

        // Save new color
        teleporterColor = newColor;

        // Set platform name
        colorIndex = -1;
        setName(newColor);

        // Needs to be here in order to save changes made in editor
        EditorUtility.SetDirty(teleporterController);
    }

    private void setName(ColorEnum newColor)
    {
        if (colorIndex == -1)
        {
            // Find all PlatformCustomizers in the scene (including inactives)
            TeleporterCustomizer[] allTeleporters = FindObjectsByType<TeleporterCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            // Creates a list and populates it with all the platforms that already exist with that color
            List<TeleporterCustomizer> coloredTeleporters = new List<TeleporterCustomizer>(50);


            // Fill newly created list with null entries
            while (coloredTeleporters.Count < coloredTeleporters.Capacity)
            {
                coloredTeleporters.Add(null);
            }


            foreach (TeleporterCustomizer teleporter in allTeleporters)
            {
                if (teleporter.teleporterColor == newColor && teleporter.colorIndex != -1)
                {
                    coloredTeleporters[teleporter.colorIndex] = teleporter;
                }
            }

            // Finds first null position in the list and inserts the new platform
            int firstNullSpace = coloredTeleporters.FindIndex(item => item == null);

            if (firstNullSpace == -1)
            {
                firstNullSpace = coloredTeleporters.Count;
            }

            // Assigns that value to colorIndex
            colorIndex = firstNullSpace;

        }

        // Sets object name according to teleporter color and amount of teleporters already with that color
        gameObject.name = teleporterColor.ToString() + " Teleporter " + (colorIndex + 1);

    }

    private bool isThereObjectInSceneWithSameName()
    {
        bool result = false;

        TeleporterCustomizer[] allTeleporters = FindObjectsByType<TeleporterCustomizer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (TeleporterCustomizer teleporter in allTeleporters)
        {
            if (teleporter.teleporterColor == teleporterColor && teleporter.gameObject.name.Equals(gameObject.name))
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
                setName(teleporterColor);
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
        if (teleporterController != null)
        {
            changeTeleporterColor(teleporterColor);
        }
    }

    /*
    *   Get / Set Functions
    */
    public void setPairedTeleporter(TeleporterCustomizer newPairedTeleporterCustomizer)
    {
        if (newPairedTeleporterCustomizer != this && newPairedTeleporterCustomizer != null)
        {
            pairedTeleporterCustomizer = newPairedTeleporterCustomizer;
            teleporterController.setPairedTeleporter(newPairedTeleporterCustomizer.getPairedTeleporterController());
        }
        else
        {
            pairedTeleporterCustomizer = null;
            teleporterController.setPairedTeleporter(null);
        }

        EditorUtility.SetDirty(teleporterController);
    }

    public TeleporterController getPairedTeleporterController()
    {
        return teleporterController;
    }
#endif
}


/******************************************************************************
*       EDITOR SUPPORT CLASS FOR PLATFORM CUSTOMIZER
*******************************************************************************/

#if UNITY_EDITOR
[CustomEditor(typeof(TeleporterCustomizer))]
public class TeleporterEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var customizer = (TeleporterCustomizer)target;

        EditorGUI.BeginChangeCheck();
        ColorEnum newColor = (ColorEnum)EditorGUILayout.EnumPopup("Teleporter Color", customizer.teleporterColor);
        TeleporterCustomizer pairedTeleporter = (TeleporterCustomizer)EditorGUILayout.ObjectField("Paired Teleporter", customizer.pairedTeleporterCustomizer, typeof(TeleporterCustomizer), true);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(customizer, "Changed Teleporter Parameters");

            customizer.changeTeleporterColor(newColor);
            customizer.setPairedTeleporter(pairedTeleporter);

            EditorUtility.SetDirty(customizer); // Make sure changes are saved
        }
    }
}
#endif
