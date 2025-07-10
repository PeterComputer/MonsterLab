using UnityEngine;

public class KeypadAreaController : Obstacle
{

    private KeypadControllerV2 keypad;

    public override void interactWith()
    {
        keypad.doKeypadPress(this);
    }

    void Awake()
    {
        if (keypad == null)
        {
            keypad = GameObject.FindGameObjectWithTag("CodedDoor").GetComponent<KeypadControllerV2>();
        }
    }
}
