using System.Threading;
using UnityEngine;

public class FlatDoorController : Obstacle
{
    [HideInInspector][SerializeField] bool startsOpen;
    [HideInInspector][SerializeField] private int pickupsLeft;
    public Sprite openDoorSprite;
    [HideInInspector] private Sprite closedSprite;
    [HideInInspector] public AudioClip openDoorAudioClip;
    [HideInInspector] public GameObject fx;

    private GameManager gameManager;

    private BoxCollider boxCollider;
    private SpriteRenderer spriteRenderer;
    [HideInInspector][SerializeField] private DoorRotationController rightDoorFrame;
    [HideInInspector][SerializeField] private DoorRotationController leftDoorFrame;
    [HideInInspector][SerializeField] private GameObject doorBackdrop;


    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        closedSprite = spriteRenderer.sprite;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startsOpen)
        {
            openDoor();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void decreasePickupsLeft()
    {

        switch (pickupsLeft)
        {
            case -1:
                break;
            case 0:
                pickupsLeft--;
                openDoor();
                break;
            case 1:
                pickupsLeft = -1;
                openDoor();
                break;

            case > 1:
                pickupsLeft--;
                break;
        }
    }

    private void openDoor()
    {
        boxCollider.isTrigger = true;
        spriteRenderer.sprite = openDoorSprite;
        SoundFXManager.instance.PlaySoundFXClip(openDoorAudioClip, transform, 1f);
        setDoorFramesActive(true);
        doorBackdrop.SetActive(true);
        fx.SetActive(true);
    }

    private void closeDoor()
    {
        pickupsLeft = 0;
        boxCollider.isTrigger = false;
        spriteRenderer.sprite = closedSprite;
        setDoorFramesActive(false);
        doorBackdrop.SetActive(false);
    }

    public void switchDoorState()
    {
        if (pickupsLeft == -1)
        {
            closeDoor();
        }
        else
        {
            decreasePickupsLeft();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.showScreenshotScreen)
        {
            gameManager.displayMonsterCompleteUI();
        }
        else
        {
            gameManager.displayEndOfLevelUI();
        }

    }

    private void setDoorFramesActive(bool active)
    {

        float angle;

        if (active)
        {
            angle = 150f;
        }
        else
        {
            angle = 0f;
        }

        rightDoorFrame.rotateToXDegrees(-angle, active);
        leftDoorFrame.rotateToXDegrees(angle, active);

        rightDoorFrame.gameObject.SetActive(active);
        leftDoorFrame.gameObject.SetActive(active);
    }
    

    public override void interactWith()
    {
        decreasePickupsLeft();
    }    
}
