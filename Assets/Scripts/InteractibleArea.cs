using PathCreation.Examples;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleArea : MonoBehaviour
{
    [SerializeField][HideInInspector] private UnityEvent _onTriggerEnter;
    [HideInInspector] public Sprite pressedSprite;
    [HideInInspector] public Material pressedWireMaterial;
    [HideInInspector] public AudioClip pressedAudioClip;
    [SerializeField][HideInInspector] private SpriteRenderer spriteRenderer;

    [HideInInspector] public bool wireStaysOn;
    [SerializeField][HideInInspector] private Sprite defaultSprite;
    [SerializeField][HideInInspector] private Material defaultWireMaterial;
    [SerializeField][HideInInspector] private RoadMeshCreator wire;
    private void OnTriggerEnter()
    {

        _onTriggerEnter.Invoke();

        spriteRenderer.sprite = pressedSprite;

        if (wire != null)
        {
            wire.roadMaterial = pressedWireMaterial;
            wire.TriggerUpdate();            
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
        if (wire != null && !wireStaysOn)
        {
            wire.roadMaterial = defaultWireMaterial;
            wire.TriggerUpdate();
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        wire = transform.parent.GetComponentInChildren<RoadMeshCreator>();

        if (wire != null && wire.roadMaterial != defaultWireMaterial)
        {
            wire.roadMaterial = defaultWireMaterial;            
        }

    }

    public void setWireStaysOn(bool newWireStaysOn)
    {
        wireStaysOn = newWireStaysOn;
    }

    public void changePlatformColor(Sprite newPlatformSprite, Material newWireMat, Material newPressedWireMaterial)
    {
        spriteRenderer.sprite = newPlatformSprite;

        if (wire != null)
        {
            wire.roadMaterial = newWireMat;
            wire.TriggerUpdate();
        }

        defaultSprite = newPlatformSprite;
        defaultWireMaterial = newWireMat;
        pressedWireMaterial = newPressedWireMaterial;


#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.SceneView.RepaintAll(); // Force editor to redraw with the new material
            UnityEditor.EditorUtility.SetDirty(this);
            if (wire != null) UnityEditor.EditorUtility.SetDirty(wire); // Save wire material change
        }
#endif

    }
}
