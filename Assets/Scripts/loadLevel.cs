using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadLevel : MonoBehaviour
{
    [SerializeField]
    private string levelID;
    [SerializeField]
    private Image completionSprite;

    public void loadLevelID() {
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().loadSceneByName(levelID);
    }

    public void Awake()
    {
        completionSprite.enabled = false;
    }

    public void Start()
    {
        Debug.Log(levelID + ": " + PlayerPrefs.GetInt(levelID));
        //If player has already completed that level
        if(Convert.ToBoolean(PlayerPrefs.GetInt(levelID))) {
            completionSprite.enabled = true;
        };
    }
}
