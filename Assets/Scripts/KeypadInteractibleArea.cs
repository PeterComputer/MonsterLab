using UnityEngine;

public class KeypadInteractibleArea : MonoBehaviour
{

    public KeypadController keypadController;
    public GameObject keypadLight;

    public Animator keypadLightAnimator;

    [SerializeField]    
    private bool isAnimating;
    
    private void OnTriggerEnter() {
        //gameObject.GetComponent<BoxCollider> ().enabled = false;
        keypadController.doKeypadPress(this);
    }

    void Awake()
    {
     keypadController = GameObject.FindGameObjectWithTag("KeypadController").GetComponent<KeypadController>(); 
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
