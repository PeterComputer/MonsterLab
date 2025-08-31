//using System;
using UnityEngine;
using UnityEngine.UI;

public class loadLevel : MonoBehaviour
{
    [SerializeField]
    private string levelID;
    [SerializeField]
    private Image completionSprite;
    private Animator animator;

    public void loadLevelID() {
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().loadSceneByName(levelID);
    }

    public void Awake()
    {
        completionSprite.enabled = false;
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
    /*  
        //If player has already completed that level
        if(Convert.ToBoolean(PlayerPrefs.GetInt(levelID))) {
            completionSprite.enabled = true;
        };
    */
    }

    public string getLevelID() {
        return levelID;
    }

    public void setComplete() {
        completionSprite.enabled = true;
    }

    public void setAsNextLevel() {
        animator.SetTrigger("isNextLevel");
    }
}
