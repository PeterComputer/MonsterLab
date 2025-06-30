using System;
using UnityEngine;
using UnityEngine.UI;

public class getDebugStatusOnEnable : MonoBehaviour
{
    private GameManager gameManager;
    private Toggle toggle;

    void Awake() {
        toggle = GetComponent<Toggle>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void OnEnable() {
        toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("debugMode"));
    }
}
