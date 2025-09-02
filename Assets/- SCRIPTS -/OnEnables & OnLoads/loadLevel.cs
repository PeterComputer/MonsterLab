//using System;
using System;
using UnityEngine;
using UnityEngine.UI;

public class loadLevel : MonoBehaviour
{
    [SerializeField] private string levelID;
    [SerializeField] private Image completionMark;
    [SerializeField] private Sprite levelCompleteSprite;
    
    [SerializeField] private Animator animator;
    [SerializeField] private Color unattemptedColor;

    // Register that the player has attempted the level and load it through the GameManager
    public void loadLevelID()
    {
        PlayerPrefs.SetInt("Attempted" + levelID, 1);
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().loadSceneByName(levelID);
    }

    public void Awake()
    {
        animator = GetComponent<Animator>();

        // If the player hasn't attemped this level yet, color it grey and disable its completion mark
        // Excludes Tutorial and Wardrobe levels
        if (!Convert.ToBoolean(PlayerPrefs.GetInt("Attempted" + levelID)) && !levelID.Contains("Tutorial") && !levelID.Contains("Wardrobe"))
        {
            //GetComponent<Image>().color = unattemptedColor;
            completionMark.GetComponent<Image>().enabled = false;
        }

        animator.enabled = false;
    }

    public string getLevelID() {
        return levelID;
    }

    public void setLevelID(string newLevelID)
    {
        levelID = newLevelID;
    }

    public void setComplete()
    {
        completionMark.sprite = levelCompleteSprite;
    }

    public void setAsNextLevel() {
        // Sets the button color to white, to prevent animator funkiness
        //GetComponent<Image>().color = Color.white;

        // Re-enables the completion mark
        completionMark.GetComponent<Image>().enabled = true;

        animator.enabled = true;
        animator.SetTrigger("isNextLevel");
    }
}
