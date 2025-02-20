using PathCreation.Examples;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleArea : MonoBehaviour
{

    public GameObject[] connectedObjects;

    [SerializeField] private UnityEvent _onTriggerEnter;
    public Sprite pressedSprite;
    public Material pressedWireMaterial;
    public AudioClip pressedAudioClip;
    private SpriteRenderer spriteRenderer;

    public bool wireStaysOn;
    private Sprite defaultSprite;
    private Material defaultWireMaterial;
    private RoadMeshCreator wire;
    private void OnTriggerEnter() {

        _onTriggerEnter.Invoke();

        foreach(GameObject currentObject in connectedObjects) {
            if(currentObject != null)
            currentObject.SetActive(!currentObject.activeSelf);
        }

        spriteRenderer.sprite = pressedSprite;
        wire.roadMaterial = pressedWireMaterial;
        wire.TriggerUpdate();
        SoundFXManager.instance.PlaySoundFXClip(pressedAudioClip, transform, 1f);
        
    }

    private void OnTriggerExit() {
        spriteRenderer.sprite = defaultSprite;
        if(!wireStaysOn) {
            wire.roadMaterial = defaultWireMaterial;
            wire.TriggerUpdate();
        }
    }

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        wire = GetComponentInChildren<RoadMeshCreator>();
        defaultWireMaterial = wire.roadMaterial;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
