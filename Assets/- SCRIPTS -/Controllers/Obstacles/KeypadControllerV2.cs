using UnityEngine;

public class KeypadControllerV2 : MonoBehaviour
{
    public const int MAX_INCORRECT_ANSWERS = 2;

    public KeypadAreaController[] keypadAreas = new KeypadAreaController[4];
    public Animator[] keypadIndicators = new Animator[4];
    public FlatDoorController door;
    private int nextKeypadNumber;
    private bool isKeypadSolved;
    [SerializeField] private AudioClip correctPressClip;
    [SerializeField] private AudioClip wrongPressClip;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        isKeypadSolved = false;
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

            // Play the "correct answer" sound effect, if it exists
            if (correctPressClip != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(correctPressClip, transform, 1f);
            }

            // If it was last correct answer needed, solve the keypad
            if (++nextKeypadNumber == keypadAreas.Length)
            {
                solveKeypad();

            }
            else
            {
                keypadIndicators[nextKeypadNumber].SetTrigger("TrFlash");
            }
        }

        // Play the "wrong answer" sound effect, if it exists
        else if (wrongPressClip != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(wrongPressClip, transform, 1f);
        }
    }

    private void solveKeypad()
    {
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
