using UnityEngine;

public class KeypadControllerV2 : MonoBehaviour
{
    public const int MAX_INCORRECT_ANSWERS = 2;

    public KeypadAreaController[] keypadAreas = new KeypadAreaController[4];
    public Animator[] keypadIndicators = new Animator[4];
    public FlatDoorController door;
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
        nextKeypadNumber = 0;
        keypadIndicators[nextKeypadNumber].SetTrigger("TrFlash");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doKeypadPress(KeypadAreaController areaPressed)
    {
        if (isKeypadSolved)
        {
            return;
        }

        // If answer is correct
        if (areaPressed == keypadAreas[nextKeypadNumber])
        {   
            // Disable the current indicator
            keypadIndicators[nextKeypadNumber].SetTrigger("TrIdle");

            // If it was last correct answer needed, solve the keypad
            if (++nextKeypadNumber == keypadAreas.Length)
            {
                solveKeypad();
            }
            else
            {
                keypadIndicators[nextKeypadNumber].SetTrigger("TrFlash");
                answersIncorrect = 0;
            }
        }
/* 
        // If answer is incorrect, reset
        else {
            //resetKeypad();

            //
            if (answersIncorrect < MAX_INCORRECT_ANSWERS)
            {
                answersIncorrect++;
            }
            else
            {
                keypadIndicators[nextKeypadNumber].SetTrigger("TrFlash");
            }
        } */
    }

    private void resetKeypad()
    {
        disableAllIndicators();
        nextKeypadNumber = 0;
        keypadIndicators[nextKeypadNumber].SetTrigger("TrFlash");
    }

    private void solveKeypad() {

        disableAllIndicators();
        door.decreasePickupsLeft();
        isKeypadSolved = true;
    }

    private void disableAllIndicators() {
        foreach (Animator indicator in keypadIndicators)
        {
            indicator.SetTrigger("TrIdle");
        }  
    }
}
