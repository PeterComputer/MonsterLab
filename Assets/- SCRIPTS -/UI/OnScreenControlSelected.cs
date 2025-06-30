using System;
using Unity.VisualScripting;
using UnityEngine;

public class OnScreenControlSelected : MonoBehaviour
{
    [SerializeField] private GameObject[] onScreenSticks;

    [SerializeField] private bool isRightHanded;
    [SerializeField] private bool isDirectional;

    void Awake() {

        isRightHanded = Convert.ToBoolean(PlayerPrefs.GetInt("usingRightHandedControls"));
        isDirectional = Convert.ToBoolean(PlayerPrefs.GetInt("usingDirectionalControls"));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resetActiveStick();
        setActiveStick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onRightHandedChange(bool newRightHandedBool) {
        isRightHanded = newRightHandedBool;
        PlayerPrefs.SetInt("usingRightHandedControls", Convert.ToInt32(isRightHanded));
        resetActiveStick();
        setActiveStick();
    }

    public void onDirectionalChange(bool newDirectionalBool) {
        isDirectional = newDirectionalBool;
        PlayerPrefs.SetInt("usingDirectionalControls", Convert.ToInt32(isDirectional));
        resetActiveStick();
        setActiveStick();
    }

    private void resetActiveStick() {
        foreach(GameObject stick in onScreenSticks) {
            stick.SetActive(false);
        }
    }

    private void setActiveStick() {

        if(onScreenSticks.Length == 0) {
            return;
        }


        if(!isRightHanded) {
            if(!isDirectional) {
                onScreenSticks[0].SetActive(true);
            }
            else {
                onScreenSticks[1].SetActive(true);
            }
        }
        else {
            if(!isDirectional) {
                onScreenSticks[2].SetActive(true);
            }
            else {
                onScreenSticks[3].SetActive(true);
            }
        }
    }

    public bool getIsRightHandedBool() {
        return isRightHanded;
    }

    public bool getIsDirectional() {
        return isDirectional;
    }

}
