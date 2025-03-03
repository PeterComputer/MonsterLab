using Enums;
using UnityEngine;
using UnityEngine.UI;

public class PartButton : MonoBehaviour
{
    private Image buttonImage;
    private GameObject[] displays;
    [SerializeField]
    private PickupType pickupType;

    void Awake()
    {
     buttonImage = GetComponent<Image>();

     switch (pickupType) {
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

    public void updateDisplayImages() {
        foreach (GameObject display in displays) {
            display.GetComponent<Image>().sprite = buttonImage.sprite;
        }
    }
}
