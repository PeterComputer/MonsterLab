using PathCreation.Examples;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class InteractibleArea : MonoBehaviour
{
    [SerializeField][HideInInspector] private UnityEvent _onTriggerEnter;
    [HideInInspector] public Sprite pressedSprite;
    public Material pressedWireMaterial;
    public AudioClip pressedAudioClip;
    [SerializeField][HideInInspector] private SpriteRenderer spriteRenderer;

    public bool wireStaysOn;
    [SerializeField][HideInInspector] private Sprite defaultSprite;
    [SerializeField][HideInInspector] private Material defaultWireMaterial;
    [SerializeField][HideInInspector] private RoadMeshCreator wire;
    private void OnTriggerEnter()
    {

        _onTriggerEnter.Invoke();

        spriteRenderer.sprite = pressedSprite;
        wire.roadMaterial = pressedWireMaterial;
        wire.TriggerUpdate();

        //pressedAudioClip can be null if multiple platforms are stacked on top of another (in order to reduce repeated noise)
        if (pressedAudioClip != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(pressedAudioClip, transform, 1f);
        }
        

    }

    private void OnTriggerExit()
    {
        spriteRenderer.sprite = defaultSprite;
        if (!wireStaysOn)
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

        if (wire.roadMaterial != defaultWireMaterial)
        {
            wire.roadMaterial = defaultWireMaterial;            
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changePlatformColor(Sprite newPlatformSprite, Material newWireMat)
    {
        spriteRenderer.sprite = newPlatformSprite;
        wire.roadMaterial = newWireMat;
        wire.TriggerUpdate();

        defaultSprite = newPlatformSprite;
        defaultWireMaterial = newWireMat;

        #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.SceneView.RepaintAll(); // Force editor to redraw with the new material
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.EditorUtility.SetDirty(wire); // Save wire material change
            }
        #endif

    }
}
