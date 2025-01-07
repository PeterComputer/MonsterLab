using Enums;
using UnityEngine;
using UnityEngine.UI;

public class PartSelector : MonoBehaviour
{
    [SerializeField]
    private PickupType pickupType;
    [SerializeField]
    private Sprite[] bodyPartImages;
    [SerializeField]
    private Image partSelectorDisplay;
    [SerializeField]
    private Image missionCardDisplay;
    private int currentImageCount;
    private GameManager gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        currentImageCount = 0;
        updateImageDisplays(currentImageCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeToPreviousPart() {
        
        if (currentImageCount == 0) {
            currentImageCount = bodyPartImages.Length -1;         
        }
        else {
            currentImageCount--;
        }

        updateImageDisplays(currentImageCount);
    }

    public void changeToNextPart() {
        if (currentImageCount == bodyPartImages.Length -1){
            currentImageCount = 0;         
        }
        else {
            currentImageCount++;
        }

        updateImageDisplays(currentImageCount);        
    }

    private void updateImageDisplays(int i) {
        partSelectorDisplay.sprite = bodyPartImages[i];
        missionCardDisplay.sprite = bodyPartImages[i]; 
    }

    public void advanceToNextMission() {
        gameManager.advanceToNextMission(pickupType);
    }
}
