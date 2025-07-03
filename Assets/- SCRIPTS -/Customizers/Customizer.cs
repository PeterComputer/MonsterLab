using UnityEngine;
using Enums;
using UnityEditor;
using System.Collections.Generic;
using System;

public class Customizer : MonoBehaviour
{
    [SerializeField]
    public ColorEnum objectColor;
    public int colorIndex = -1;

#if UNITY_EDITOR
    protected bool isThereObjectInSceneWithSameName<T>() where T : Customizer
    {
        bool result = false;

        Customizer[] allObjectsOfType = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Customizer obj in allObjectsOfType)
        {
            if (obj.objectColor == objectColor && obj.gameObject.name.Equals(gameObject.name) && !obj.gameObject.Equals(this.gameObject))
            {
                result = true;
                break;
            }
        }
        return result;
    }

    protected void setName<T>(ColorEnum newColor, String objName) where T : Customizer
    {
        if (colorIndex == -1)
        {
            // Find all xCustomizers in the scene (including inactives)
            Customizer[] allObjects = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            // Creates a list and populates it with all the platforms that already exist with that color
            List<Customizer> coloredObjects = new List<Customizer>(50);


            // Fill newly created list with null entries
            while (coloredObjects.Count < coloredObjects.Capacity)
            {
                coloredObjects.Add(null);
            }


            foreach (Customizer obj in allObjects)
            {
                if (obj.objectColor == newColor && obj.colorIndex != -1)
                {


                    coloredObjects[obj.colorIndex] = obj;
                }
            }

            // Finds first null position in the list and inserts the new obj
            int firstNullSpace = coloredObjects.FindIndex(item => item == null);

            if (firstNullSpace == -1)
            {
                firstNullSpace = coloredObjects.Count;
            }

            // Assigns that value to colorIndex
            colorIndex = firstNullSpace;

        }

        // Sets object name according to obj color and amount of platforms already with that color
        gameObject.name = objectColor.ToString() + " " + objName + " " + (colorIndex + 1);

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

    protected virtual void OnUndoRedo() { }

#endif
}
