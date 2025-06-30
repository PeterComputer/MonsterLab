using System;
using UnityEngine;

public class SaveStateController : MonoBehaviour
{
    [SerializeField]
    private loadLevel[] levelButtons;

    void Awake()
    {
        levelButtons = gameObject.GetComponentsInChildren<loadLevel>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool foundNextLevel = false;

        foreach (loadLevel button in levelButtons) {
            //If player has already completed that level
            if(Convert.ToBoolean(PlayerPrefs.GetInt(button.getLevelID()))) {
                button.setComplete();
            }
            else if (!foundNextLevel) {
                button.setAsNextLevel();
                foundNextLevel = true;
            }
        }
    }
}
