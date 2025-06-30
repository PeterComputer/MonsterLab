using UnityEngine;
using UnityEngine.UI;

public class getToggleStatusOnEnable : MonoBehaviour
{   
    [SerializeField] private int toggleType;
    private OnScreenControlSelected onScreenControls;
    private Toggle toggle;
    
    void Awake() {
        toggle = GetComponent<Toggle>();

        GameObject androidUI = GameObject.FindGameObjectWithTag("AndroidUI");

        //If on a level scene, AndroidUI will have the <OnScreenControlSelected> script
        if(androidUI != null) {
            onScreenControls = GameObject.FindGameObjectWithTag("AndroidUI").GetComponent<OnScreenControlSelected>();
        }

        //If on the main menu scene, GameController will have the <OnScreenControlSelected> script
        else {
            onScreenControls = GameObject.FindGameObjectWithTag("GameController").GetComponent<OnScreenControlSelected>();
        }
        
    }

    private void OnEnable() {
        switch (toggleType) {
            case 0:
                toggle.isOn = onScreenControls.getIsRightHandedBool();
                break;
            case 1:
                toggle.isOn = onScreenControls.getIsDirectional();
                break;
        }
    }    
}
