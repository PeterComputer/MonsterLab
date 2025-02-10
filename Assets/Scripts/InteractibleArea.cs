using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleArea : MonoBehaviour
{

    public GameObject[] connectedObjects;

    [SerializeField] private UnityEvent _onTriggerEnter;
    public Sprite pressedSprite;
    public AudioClip pressedAudioClip;
    private SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;
    private void OnTriggerEnter() {

        _onTriggerEnter.Invoke();

        foreach(GameObject currentObject in connectedObjects) {
            if(currentObject != null)
            currentObject.SetActive(!currentObject.activeSelf);
        }

        spriteRenderer.sprite = pressedSprite;
        SoundFXManager.instance.PlaySoundFXClip(pressedAudioClip, transform, 1f);
        
    }

    private void OnTriggerExit() {
        spriteRenderer.sprite = defaultSprite;
    }

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
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
