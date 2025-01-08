using UnityEngine;
using UnityEngine.SceneManagement;
using Enums;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject headSelectionUI;
    [SerializeField]
    private GameObject torsoSelectionUI;
    [SerializeField]
    private GameObject legsSelectionUI;
    [SerializeField]
    private GameObject headMissionUI;
    [SerializeField]
    private GameObject torsoMissionUI;
    [SerializeField]
    private GameObject legsMissionUI;
    [SerializeField]
    private GameObject[] pickups; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    public void loadMainMenuScene() {
        SceneManager.LoadScene("Main Menu");
    }

    public void reloadCurrentScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void loadNextScene() {
        //if not at the final scene
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else {
            reloadCurrentScene();
        }
    }

    public void openSelectionUI(PickupType type) {
        switch (type) {
            case PickupType.head:
            headSelectionUI.SetActive(true);
            break;
            case PickupType.torso:
            torsoSelectionUI.SetActive(true);
            break;
            case PickupType.legs:
            legsSelectionUI.SetActive(true);
            break;                        
        }
    }

    public void advanceToNextMission(PickupType currentType) {

        //This probably shouldn't be hardcoded, but it'll work
        switch(currentType) {
            case PickupType.head:
                headMissionUI.SetActive(false);
                torsoMissionUI.SetActive(true);
                setActiveTypeOfPickup(PickupType.torso);
            break;
            case PickupType.torso:
                torsoMissionUI.SetActive(false);
                legsMissionUI.SetActive(true);
                setActiveTypeOfPickup(PickupType.legs);
            break;
            case PickupType.legs:
                legsMissionUI.SetActive(false);
            break;
        }
        
        destroyTypeOfPickup(currentType);
        
        GameObject.FindWithTag("Door").GetComponent<DoorController>().decreasePickupsLeft();
    }


    private void destroyTypeOfPickup(PickupType type) {
        foreach (GameObject pickup in pickups) {
            if (pickup != null && pickup.GetComponent<PickupController>().getPickupType() == type) {
                Destroy(pickup);
            }
        }
    }

    private void setActiveTypeOfPickup(PickupType type) {
        foreach (GameObject pickup in pickups) {
            if (pickup != null && pickup.GetComponent<PickupController>().getPickupType() == type) {
                pickup.SetActive(true);
            }
        }        
    }

    public void closeApplication() {
        Application.Quit();
    }
}
