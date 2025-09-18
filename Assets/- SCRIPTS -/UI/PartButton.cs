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

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        partSelectorGrid = gameObject.GetComponentInParent<PartSelectorGrid>();

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void doInteraction()
    {
        partSelectorGrid.resetSelectedButton();
        transform.parent.GetComponent<Animator>().SetTrigger("TrSelected");
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
}
