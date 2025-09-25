using UnityEngine;
using UnityEngine.UI;

public class CodedDoorIndicatorController : MonoBehaviour
{
    [SerializeField] private GameObject outline;
    [SerializeField] private Material disabledMaterial;
    private SpriteRenderer spriteRenderer;
    private Material enabledMaterial;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enabledMaterial = spriteRenderer.material;

        // Set the visual state of the object to its disabled state
        spriteRenderer.material = disabledMaterial;        
    }

    public void enableIndicator()
    {
        spriteRenderer.material = enabledMaterial;
        animator.SetTrigger("TrFlash");
        outline.SetActive(true);
    }

    public void disableIndicator()
    {
        animator.SetTrigger("TrIdle");
        outline.SetActive(false);
    }
    

}
