using System.Threading;
using UnityEngine;

public class FlatDoorController : MonoBehaviour
{
    [SerializeField] bool startsOpen;    
    [SerializeField]private int pickupsLeft;
    public Sprite openDoorSprite;
    private Sprite closedSprite;
    public AudioClip openDoorAudioClip;
    
    private GameManager gameManager;
    
    private BoxCollider boxCollider;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        closedSprite = spriteRenderer.sprite;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(startsOpen) {
            openDoor();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void decreasePickupsLeft() {

        switch(pickupsLeft) {
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

            case >1:
                pickupsLeft--;
                break;
        }
    }

    private void openDoor() {
        boxCollider.isTrigger = true;
        spriteRenderer.sprite = openDoorSprite;
        SoundFXManager.instance.PlaySoundFXClip(openDoorAudioClip, transform, 1f);

    }

    private void closeDoor() {
        pickupsLeft = 0;
        boxCollider.isTrigger = false;
        spriteRenderer.sprite = closedSprite;
    }

    public void switchDoorState() {
        if (pickupsLeft == -1) {
            closeDoor();
        }
        else {
            decreasePickupsLeft();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(gameManager.isTutorial) {
            gameManager.displayMonsterCompleteUI();
        }
        else {
            gameManager.loadNextScene();
        }
        
    }
}
