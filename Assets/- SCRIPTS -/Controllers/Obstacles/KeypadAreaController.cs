using UnityEngine;

public class KeypadAreaController : Obstacle
{

    [SerializeField] private KeypadControllerV2 keypad;

    public override void interactWith()
    {
        keypad.doKeypadPress(this);
    }
}
