using UnityEngine;
using UnityEngine.UI;

public class getToggleStatusOnEnable : MonoBehaviour
{   
    [SerializeField] private int toggleType;
    private OnScreenControlSelected onScreenControls;
    private Toggle toggle;
    
    void Awake() {
        toggle = GetComponent<Toggle>();
        onScreenControls = GameObject.FindGameObjectWithTag("AndroidUI").GetComponent<OnScreenControlSelected>();
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
