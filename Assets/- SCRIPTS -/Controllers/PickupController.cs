using UnityEngine;
using Enums;
using UnityEngine.UI;

public class PickupController : MonoBehaviour
{

    [SerializeField] private PickupType type;
    [SerializeField] private Image imageToDisable;
    [SerializeField] private AudioClip pickupSoundFX;
    private GameManager gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        if (type != PickupType.other)
        {
            gameManager.switchPlayerInputMap();
            gameManager.openSelectionUI(type);

            if (imageToDisable != null)
            {
                imageToDisable.enabled = false;
            }

            if (pickupSoundFX != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(pickupSoundFX, transform, 1f);
            }

        }

        else
        {
            GameObject.FindWithTag("Door").GetComponent<FlatDoorController>().decreasePickupsLeft();
            Destroy(this.gameObject);
        }
    }

    public PickupType getPickupType() {
        return type;
    }
}
