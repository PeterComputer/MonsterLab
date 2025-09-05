using System.Collections.Generic;
using PathCreation.Examples;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class InteractibleArea : MonoBehaviour
{
    [SerializeField][HideInInspector] private UnityEvent _onTriggerEnter;
    [HideInInspector] public Sprite pressedSprite;
    [HideInInspector] public Material pressedWireMaterial;
    public AudioClip pressedAudioClip;
    [SerializeField][HideInInspector] private SpriteRenderer spriteRenderer;
    [HideInInspector] public bool wireStaysOn;
    [SerializeField][HideInInspector] private Sprite defaultSprite;
    [SerializeField][HideInInspector] private Material defaultWireMaterial;
    [SerializeField]private List<GameObject> wireObjects;
    [SerializeField]private List<RoadMeshCreator> wireScripts;
    //[SerializeField][HideInInspector] private GameObject wireObject;
    //[SerializeField][HideInInspector] private RoadMeshCreator wireScript;
    [SerializeField][HideInInspector] private GameObject wirePrefab;
    private void OnTriggerEnter()
    {

        _onTriggerEnter.Invoke();

        spriteRenderer.sprite = pressedSprite;

        // Change the wire materials to their pressed state
        foreach (GameObject wireObject in wireObjects)
        {
            if (wireObject.activeSelf)
            {
                RoadMeshCreator wireScript = wireObject.GetComponent<RoadMeshCreator>();
                wireScript.roadMaterial = pressedWireMaterial;
                wireScript.TriggerUpdate();
            }
        }

        // Play an audioClip when the platform is pressed
        // pressedAudioClip can be null if multiple platforms are stacked on top of another (in order to reduce repeated noise)
        if (pressedAudioClip != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(pressedAudioClip, transform, 1f);
        }
    }

    private void OnTriggerExit()
    {
        spriteRenderer.sprite = defaultSprite;

        foreach (GameObject wireObject in wireObjects)
        {
            if (wireObject.activeSelf && !wireStaysOn)
            {
                RoadMeshCreator wireScript = wireObject.GetComponent<RoadMeshCreator>();
                wireScript.roadMaterial = defaultWireMaterial;
                wireScript.TriggerUpdate();
            }
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;

        checkForExistingWires();

        // Check whether wires should be active or not
        bool hasWire = false;

        foreach (GameObject wire in wireObjects)
        {
            if (wire.activeSelf && wire.GetComponent<RoadMeshCreator>().enabled)
            {
                hasWire = true;
            }
        }

        setHasWire(hasWire);


        // If wire's material does not correspond to the default wire material
        foreach (RoadMeshCreator wireScript in wireScripts)
        {
            if (wireScript.roadMaterial != defaultWireMaterial)
            {
                // Update it
                wireScript.roadMaterial = defaultWireMaterial;
            }
        }
    }

    public void setHasWire(bool hasWire)
    {
        // Remove empty entries from wires that might have been deleted by the player
        removeEmptyEntries();

        // Check and add any new wires that might have been placed by the developer
        checkForExistingWires();        

#if UNITY_EDITOR
        // If no wires exist, create one at the start of wireObjects and disable it
        if (wireObjects.Count == 0)
        {
            createNewWire();
        }
#endif
        // Set all wires to the hasWire value passed in the parameter
        foreach (GameObject wireObject in wireObjects)
        {
            wireObject.SetActive(hasWire);
        }
    }

#if UNITY_EDITOR
    private void createNewWire()
    {
        GameObject newWireObject = (GameObject)PrefabUtility.InstantiatePrefab(wirePrefab, gameObject.transform.parent);

        // The wire transform position needs a 5 at the z coordinate for reasons that I can't fully explain
        newWireObject.transform.localPosition = new Vector3(0, 0, 5);

        wireObjects[0] = newWireObject;
        wireScripts[0] = newWireObject.GetComponent<RoadMeshCreator>();
    }
#endif

    private void removeEmptyEntries()
    {
        foreach (GameObject wireObject in wireObjects)
        {
            if (wireObject == null)
            {
                wireObjects.Remove(wireObject);
                wireScripts.Remove(wireObject.GetComponent<RoadMeshCreator>());
            }
        }
    }

    private void checkForExistingWires()
    {
        // Get the InteractibleArea's wire brothers 
        for (int currentChildI = 0; currentChildI < transform.parent.childCount; currentChildI++)
        {
            GameObject currentChild = transform.parent.GetChild(currentChildI).gameObject;

            if (currentChild.tag == "Wire" && !wireObjects.Contains(currentChild)) //and there isnt a repeat object
            {
                wireObjects.Add(currentChild);
                wireScripts.Add(currentChild.GetComponent<RoadMeshCreator>());
            }
        }    
    }

    public bool getHasWire()
    {
        // Remove empty entries from wires that might have been deleted by the developer
        removeEmptyEntries();

        // Check and add any new wires that might have been placed by the developer
        checkForExistingWires();

#if UNITY_EDITOR
        // If no wires exist, create one at the start of wireObjects and disable it
        if (wireObjects.Count == 0)
        {
            createNewWire();
            setHasWire(false);
        }
#endif
        bool result = wireObjects[0].activeSelf;

        return result;
    }

    public void setWireStaysOn(bool newWireStaysOn)
    {
        wireStaysOn = newWireStaysOn;
    }

    public void changePlatformColor(Sprite newPlatformSprite, Sprite newPressedSprite, Material newWireMat, Material newPressedWireMaterial)
    {
        spriteRenderer.sprite = newPlatformSprite;

        // Remove empty entries from wires that might have been deleted by the player
        removeEmptyEntries();

        // If the wires are active (they are either ALL active or ALL inactive)
        if (wireObjects[0].activeSelf)
        {
            // Change the road material for each of them and update the script
            foreach (RoadMeshCreator wireScript in wireScripts)
            {
                wireScript.roadMaterial = newWireMat;
                wireScript.TriggerUpdate();
            }
        }

        defaultSprite = newPlatformSprite;
        pressedSprite = newPressedSprite;
        defaultWireMaterial = newWireMat;
        pressedWireMaterial = newPressedWireMaterial;

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.SceneView.RepaintAll(); // Force editor to redraw with the new material
            UnityEditor.EditorUtility.SetDirty(this);

            // If the wires are active (they are either ALL active or ALL inactive)
            if (wireObjects[0].activeSelf)
            {
                // Save the wire material change in each wireScript
                foreach (RoadMeshCreator wireScript in wireScripts)
                {
                    UnityEditor.EditorUtility.SetDirty(wireScript);
                }
            }

        }
#endif
    }
}
