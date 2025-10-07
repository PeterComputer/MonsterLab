using Enums;
using UnityEngine;
using UnityEngine.UI;

public class PartButton : MonoBehaviour
{
    private Image buttonImage;
    [SerializeField] private Sprite playerSprite;
    private GameObject[] displays;
    [SerializeField] private PickupType pickupType;
    private GameManager gameManager;
    private PartSelectorGrid partSelectorGrid;
    [SerializeField] private Transform parentTransform;
    private bool isSelected;

    // === Scale logic ===
    private Vector3 originalScale;
    private Vector3 scaledUpScale;

    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float scaleSpeed = 2f; // How fast the pulsing happens

    private float pulseTimer = 0f;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        partSelectorGrid = gameObject.GetComponentInParent<PartSelectorGrid>();
        parentTransform = transform.parent;

        originalScale = parentTransform.localScale;
        scaledUpScale = originalScale * scaleMultiplier;

        switch (pickupType)
        {
            case PickupType.head:
                displays = GameObject.FindGameObjectsWithTag("HeadDisplay");
                break;
            case PickupType.torso:
                displays = GameObject.FindGameObjectsWithTag("TorsoDisplay");
                break;
            case PickupType.legs:
                displays = GameObject.FindGameObjectsWithTag("LegsDisplay");
                break;
        }
    }

    void Update()
    {
        if (isSelected)
        {
            // Increase pulse timer
            pulseTimer += Time.deltaTime * scaleSpeed;

            // Use Mathf.PingPong to create smooth looping effect
            float t = Mathf.PingPong(pulseTimer, 1f);

            parentTransform.localScale = Vector3.Lerp(originalScale, scaledUpScale, t);
        }
        else
        {
            // Smoothly return to original scale when not selected
            parentTransform.localScale = Vector3.Lerp(parentTransform.localScale, originalScale, Time.deltaTime * scaleSpeed);

            // Reset pulse timer
            pulseTimer = 0f;
        }
    }

    public void doInteraction()
    {
        partSelectorGrid.resetSelectedButton();
        isSelected = true;
        updateDisplayImages();
    }

    public void updateDisplayImages()
    {
        foreach (GameObject display in displays)
        {
            display.GetComponent<Image>().sprite = buttonImage.sprite;
        }

        gameManager.updatePlayerSprite(pickupType, buttonImage.sprite, playerSprite);
    }

    public void setIsSelected(bool newIsSelected)
    {
        isSelected = newIsSelected;
    }
}