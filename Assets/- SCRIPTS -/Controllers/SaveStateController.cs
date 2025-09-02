using System;
using UnityEngine;

// This class sets the save state for each individual level button, depending on the information saved in PlayerPrefs
public class SaveStateController : MonoBehaviour
{
    [SerializeField] private loadLevel[] levelButtons;

    void Awake()
    {
        levelButtons = gameObject.GetComponentsInChildren<loadLevel>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setLevelStates();
    }

    public void setLevelStates()
    {
        bool foundNextLevel = false;

        foreach (loadLevel button in levelButtons)
        {
            //If player has already completed that level
            if (Convert.ToBoolean(PlayerPrefs.GetInt(button.getLevelID())))
            {
                button.setComplete();
            }
            else if (!foundNextLevel)
            {
                button.setAsNextLevel();
                foundNextLevel = true;
            }
        }
    }
}
