using Enums;
using UnityEngine;
using UnityEngine.UI;

public class getImageOnEnable : MonoBehaviour
{
    private Image displayImage;
    private GameManager gameManager;
    [SerializeField]
    private PickupType pickupType;

    private void OnEnable() {
     switch (pickupType) {
            case PickupType.head:
                if (gameManager.currentHead != null) {
                    displayImage.sprite = gameManager.currentHead;
                }
                break;
            case PickupType.torso:
                if (gameManager.currentTorso != null) {
                    displayImage.sprite = gameManager.currentTorso;
                }
                break;
            case PickupType.legs:
                if (gameManager.currentLegs != null) {
                    displayImage.sprite = gameManager.currentLegs;
                }
                break;
        }
    }

    void Awake() {
        displayImage = GetComponent<Image>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }
}
