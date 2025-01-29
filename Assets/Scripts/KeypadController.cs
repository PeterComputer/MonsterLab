using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class KeypadController : MonoBehaviour
{
    public const int MAX_INCORRECT_ANSWERS = 2;

    public KeypadInteractibleArea[] interactibleAreas = new KeypadInteractibleArea[4];
    public GameObject door;
    [SerializeField]
    private int nextKeypadNumber;
    [SerializeField]
    private bool isKeypadSolved;
    [SerializeField]
    private int answersIncorrect;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        isKeypadSolved = false;
        answersIncorrect = 0;
        resetKeypad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doKeypadPress(KeypadInteractibleArea areaPressed) {

        areaPressed.setAreaEnabled(false);

        if(isKeypadSolved) {
            return;
        }

        // If answer is correct
        if (areaPressed == interactibleAreas[nextKeypadNumber]) {


            // If it was last correct answer needed, solve the keypad
            if(++nextKeypadNumber == interactibleAreas.Length) {
                solveKeypad();
            }
            else {
                interactibleAreas[nextKeypadNumber].setAreaEnabled(true);
                answersIncorrect = 0;
            }
        }

        // If answer is incorrect, reset
        else {
            resetKeypad();

            //
            if(answersIncorrect < MAX_INCORRECT_ANSWERS) {
                answersIncorrect++;
            }
            else {
                interactibleAreas[nextKeypadNumber].setFlashingLight(true);
            }
        }
    }

    private void resetKeypad() {
        disableAllAreas();
        nextKeypadNumber = 0;
        interactibleAreas[nextKeypadNumber].setAreaEnabled(true);        
    }

    private void solveKeypad() {

        disableAllAreas();
        door.SetActive(false);
        isKeypadSolved = true;
    }

    private void disableAllAreas() {
        foreach (KeypadInteractibleArea area in interactibleAreas) {
            area.setAreaEnabled(false);
            area.setFlashingLight(false);
        }  
    }
}
