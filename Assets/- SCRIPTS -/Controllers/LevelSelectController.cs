using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelSelectController : MonoBehaviour
{
    public List<GameObject> levelMenus;
    [SerializeField] GameObject previousLevelButton;
    [SerializeField] GameObject nextLevelButton;

    private SaveStateController saveStateController;

    [SerializeField] private int currentMenuIndex;
    private GameObject currentMenu;

    void Awake()
    {
        Debug.Log("Loading page: " + PlayerPrefs.GetInt("LevelSelectPageToLoad"));
        currentMenuIndex = PlayerPrefs.GetInt("LevelSelectPageToLoad");
        currentMenu = levelMenus[currentMenuIndex];

        for (int i = 0; i <= levelMenus.Count-1; i++)
        {
            if (levelMenus[i] == currentMenu)
            {
                levelMenus[i].SetActive(true);
                if (i == 0) previousLevelButton.SetActive(false);
                else if (i == levelMenus.Count - 1) nextLevelButton.SetActive(false);
            }
            else levelMenus[i].SetActive(false);
        }

        saveStateController = GetComponent<SaveStateController>();
    }

    // Called when player switches scenes, saves the last page loaded
    void OnDestroy()
    {
        PlayerPrefs.SetInt("LevelSelectPageToLoad", currentMenuIndex);
        PlayerPrefs.Save();
    }

    public void goToNextLevelMenu()
    {
        if (++currentMenuIndex > levelMenus.Count - 1) currentMenuIndex = levelMenus.Count - 1;

        currentMenu.SetActive(false);

        if (currentMenuIndex < levelMenus.Count - 1)
        {
            nextLevelButton.SetActive(true);
            previousLevelButton.SetActive(true);
        }

        if (currentMenuIndex == levelMenus.Count - 1)
        {
            nextLevelButton.SetActive(false);
            previousLevelButton.SetActive(true);
        }

        currentMenu = levelMenus[currentMenuIndex];
        currentMenu.SetActive(true);
        saveStateController.setLevelStates();

        PlayerPrefs.SetInt("LevelSelectPageToLoad", currentMenuIndex);
        PlayerPrefs.Save();
    }

    public void goToPreviousLevelMenu()
    {
        if (--currentMenuIndex < 0) currentMenuIndex = 0;

        currentMenu.SetActive(false);

        if (currentMenuIndex > 0)
        {
            nextLevelButton.SetActive(true);
            previousLevelButton.SetActive(true);
        }

        if (currentMenuIndex == 0)
        {
            nextLevelButton.SetActive(true);
            previousLevelButton.SetActive(false);
        }

        currentMenu = levelMenus[currentMenuIndex];
        currentMenu.SetActive(true);
        saveStateController.setLevelStates();

        PlayerPrefs.SetInt("LevelSelectPageToLoad", currentMenuIndex);
        PlayerPrefs.Save();
    }
}
