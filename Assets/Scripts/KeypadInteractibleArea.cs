using UnityEngine;

public class KeypadInteractibleArea : MonoBehaviour
{

    public KeypadController keypadController;
    public GameObject keypadLight;
    public Animator keypadLightAnimator;
    public Sprite pressedSprite;
    public AudioClip pressedAudioClip;
  
    private bool isAnimating;
    private SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;
    
    private void OnTriggerEnter() {
        //gameObject.GetComponent<BoxCollider> ().enabled = false;
        keypadController.doKeypadPress(this);
        spriteRenderer.sprite = pressedSprite;
        SoundFXManager.instance.PlaySoundFXClip(pressedAudioClip, transform, 1f);
    }

    private void OnTriggerExit() {
        spriteRenderer.sprite = defaultSprite;
    }

    void Awake()
    {
        keypadController = GameObject.FindGameObjectWithTag("KeypadController").GetComponent<KeypadController>();
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

   public void setAreaEnabled(bool value) {
        keypadLight.SetActive(value);
    }

    public void setFlashingLight(bool isFlashing) {
        isAnimating = isFlashing;

        if(isAnimating) {
            keypadLightAnimator.SetTrigger("TrFlash");
        }
        else {
            keypadLightAnimator.SetTrigger("TrIdle");
        }
    }
}
