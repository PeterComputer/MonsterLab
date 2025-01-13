using System;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField]
    private Image monsterCompleteDisplay; 
    [SerializeField]
    private InputActionReference nextImage;
    [SerializeField]
    private InputActionReference previousImage;
    [SerializeField]
    private InputActionReference closePartSelector;       

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
        /*
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
            advanceToNextMission();
            this.transform.parent.gameObject.SetActive(false);
        }

        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            changeToPreviousPart();
        }

        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            changeToNextPart();
        }
        */
    }

    private void OnEnable() {
        nextImage.action.performed += changeToNextPart;
        previousImage.action.performed += changeToPreviousPart;
        closePartSelector.action.performed += closeMenu;
    }

    private void OnDisable() {
        nextImage.action.performed -= changeToNextPart;
        previousImage.action.performed -= changeToPreviousPart;
        closePartSelector.action.performed -= closeMenu;        
    }

    private void changeToNextPart(InputAction.CallbackContext context)
    {
        changeToNextPart();
    }

    private void changeToPreviousPart(InputAction.CallbackContext context)
    {
        changeToPreviousPart();
    }

    private void closeMenu(InputAction.CallbackContext context)
    {
            advanceToNextMission();
            this.transform.parent.gameObject.SetActive(false);
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
        monsterCompleteDisplay.sprite = bodyPartImages[i];
    }

    public void advanceToNextMission() {
        gameManager.switchPlayerInputMap();
        gameManager.advanceToNextMission(pickupType);
    }
}
