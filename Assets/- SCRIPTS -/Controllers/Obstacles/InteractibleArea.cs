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
    [SerializeField][HideInInspector] private GameObject wireObject;
    [SerializeField][HideInInspector] private RoadMeshCreator wireScript;
    [SerializeField][HideInInspector] private GameObject wirePrefab;
    private void OnTriggerEnter()
    {

        _onTriggerEnter.Invoke();

        spriteRenderer.sprite = pressedSprite;

        if (wireObject.activeSelf)
        {
            wireScript.roadMaterial = pressedWireMaterial;
            wireScript.TriggerUpdate();
        }


        //pressedAudioClip can be null if multiple platforms are stacked on top of another (in order to reduce repeated noise)
        if (pressedAudioClip != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(pressedAudioClip, transform, 1f);
        }
    }

    private void OnTriggerExit()
    {
        spriteRenderer.sprite = defaultSprite;
        if (wireObject.activeSelf && !wireStaysOn)
        {
            wireScript.roadMaterial = defaultWireMaterial;
            wireScript.TriggerUpdate();
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;

        if (wireObject == null || !wireObject.activeSelf)
        {
            setHasWire(false);
        }
        else if (wireScript.enabled)
        {
            setHasWire(true);
        }

        wireScript = wireObject.GetComponent<RoadMeshCreator>();
        
        // If wire's material does not correspond to the default wire material
        if (wireScript.roadMaterial != defaultWireMaterial)
        {
            // Update it
            wireScript.roadMaterial = defaultWireMaterial;
        }
        
    }

    public void setHasWire(bool hasWire)
    {
        if (wireObject == null)
        {
            createNewWire();
            wireObject.SetActive(false);
        }

        // if the platform should have a wire but currently doesn't
        if (hasWire && !wireObject.activeSelf)
        {
            // if there was a wire object already before deletion, enable it again
            if (wireObject != null)
            {
                wireObject.SetActive(true);
            }
            // if there wasn't a wire object before deletion, create a new one
            else
            {
                createNewWire();
            }
        }
        //if the platform should not have a wire but does, save it in a variable for later, then delete it
        else if (!hasWire && wireObject.activeSelf)
        {
            wireObject.SetActive(false);
        }
    }

    private void createNewWire()
    {
        wireObject = (GameObject)PrefabUtility.InstantiatePrefab(wirePrefab, gameObject.transform.parent);

        // The wire transform position needs a 5 at the z coordinate for reasons that I can't fully explain
        wireObject.transform.localPosition = new Vector3(0, 0, 5);

        wireScript = wireObject.GetComponent<RoadMeshCreator>();        
    }

    public bool getHasWire()
    {
        if (wireObject == null)
        {
            createNewWire();
        }

        bool result = wireObject.activeSelf;

        return result;
    }

    public void setWireStaysOn(bool newWireStaysOn)
    {
        wireStaysOn = newWireStaysOn;
    }

    public void changePlatformColor(Sprite newPlatformSprite, Sprite newPressedSprite, Material newWireMat, Material newPressedWireMaterial)
    {
        spriteRenderer.sprite = newPlatformSprite;

        if (wireObject != null && wireObject.activeSelf)
        {   
            wireScript.roadMaterial = newWireMat;
            wireScript.TriggerUpdate();
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
            if (wireObject != null && wireObject.activeSelf) UnityEditor.EditorUtility.SetDirty(wireScript); // Save wire material change
        }
#endif

    }
}
