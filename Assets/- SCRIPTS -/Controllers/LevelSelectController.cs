using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    public List<GameObject> levelMenus;
    public List<Image> pageIndicators;
    public List<string> wardrobeLevels;
    [SerializeField] GameObject previousPageButton;
    [SerializeField] GameObject nextPageButton;
    [SerializeField] loadLevel wardrobeButton;

    private SaveStateController saveStateController;

    [SerializeField] private int currentMenuIndex;
    private GameObject currentMenu;

    public Color selectedColor;
    public Color idleColor;

    [SerializeField] private int completedLevelCount;

    [SerializeField] private int completionCheck1;
    [SerializeField] private int completionCheck2;
    [SerializeField] private int completionCheck3;

    [SerializeField] private TextMeshProUGUI progressionDisplay;


    void Start()
    {
        completedLevelCount = PlayerPrefs.GetInt("CompletedLevelCount");
        Debug.Log("completed levels: " + completedLevelCount);
        currentMenuIndex = PlayerPrefs.GetInt("LevelSelectPageToLoad");
        currentMenu = levelMenus[currentMenuIndex];

        for (int i = 0; i <= levelMenus.Count - 1; i++)
        {
            if (levelMenus[i] == currentMenu)
            {
                levelMenus[i].SetActive(true);
                wardrobeButton.setLevelID(wardrobeLevels[i]);
                pageIndicators[i].color = selectedColor;

                if (i == 0) previousPageButton.SetActive(false);
                else if (i == levelMenus.Count - 1) nextPageButton.SetActive(false);
                checkRequiredProgression();
            }
            else
            {
                levelMenus[i].SetActive(false);
                pageIndicators[i].color = idleColor;
            }

        }

        saveStateController = GetComponent<SaveStateController>();
    }

    // Called when player switches scenes, saves the last page loaded
    void OnDestroy()
    {
        PlayerPrefs.SetInt("LevelSelectPageToLoad", currentMenuIndex);
        PlayerPrefs.Save();
    }

    public void goToNextPageMenu()
    {
        pageIndicators[currentMenuIndex].color = idleColor;

        if (++currentMenuIndex > levelMenus.Count - 1) currentMenuIndex = levelMenus.Count - 1;

        currentMenu.SetActive(false);


        if (currentMenuIndex < levelMenus.Count - 1)
        {
            nextPageButton.SetActive(true);
            previousPageButton.SetActive(true);
        }

        if (currentMenuIndex == levelMenus.Count - 1)
        {
            nextPageButton.SetActive(false);
            previousPageButton.SetActive(true);
        }

        currentMenu = levelMenus[currentMenuIndex];
        wardrobeButton.setLevelID(wardrobeLevels[currentMenuIndex]);
        pageIndicators[currentMenuIndex].color = selectedColor;
        currentMenu.SetActive(true);
        saveStateController.setLevelStates();

        PlayerPrefs.SetInt("LevelSelectPageToLoad", currentMenuIndex);
        PlayerPrefs.Save();

        checkRequiredProgression();
    }

    public void goToPreviousPageMenu()
    {
        pageIndicators[currentMenuIndex].color = idleColor;

        if (--currentMenuIndex < 0) currentMenuIndex = 0;

        currentMenu.SetActive(false);


        if (currentMenuIndex > 0)
        {
            nextPageButton.SetActive(true);
            previousPageButton.SetActive(true);
        }

        if (currentMenuIndex == 0)
        {
            nextPageButton.SetActive(true);
            previousPageButton.SetActive(false);
        }

        currentMenu = levelMenus[currentMenuIndex];
        wardrobeButton.setLevelID(wardrobeLevels[currentMenuIndex]);
        pageIndicators[currentMenuIndex].color = selectedColor;
        currentMenu.SetActive(true);
        saveStateController.setLevelStates();

        PlayerPrefs.SetInt("LevelSelectPageToLoad", currentMenuIndex);
        PlayerPrefs.Save();

        checkRequiredProgression();
    }

    private void checkRequiredProgression()
    {
        int completionCheckToUse;

        switch (currentMenuIndex)
        {
            case 0:
                completionCheckToUse = completionCheck1;
                break;
            case 1:
                completionCheckToUse = completionCheck2;
                break;
            case 2:
                completionCheckToUse = completionCheck3;
                break;
            default:
                completionCheckToUse = 0;
                break;
        }

        // If player doesn't meet the progression requirement, disable the next page button and update the progression display
        if (completedLevelCount < completionCheckToUse)
        {
            nextPageButton.GetComponent<Image>().color = Color.red;
            nextPageButton.GetComponent<Button>().enabled = false;

            progressionDisplay.enabled = true;
            progressionDisplay.text = completedLevelCount + "/" + completionCheckToUse;
        }
        else
        {
            nextPageButton.GetComponent<Image>().color = Color.white;
            nextPageButton.GetComponent<Button>().enabled = true;

            progressionDisplay.enabled = false;
        }
    }
}
