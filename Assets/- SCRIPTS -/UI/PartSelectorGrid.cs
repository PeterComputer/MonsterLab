using UnityEngine;
using Enums;
using UnityEngine.UI;

public class PartSelectorGrid : MonoBehaviour
{

    [SerializeField]
    private PickupType pickupType;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    public void advanceToNextMission() {
        gameManager.advanceToNextMission(pickupType);
    }
}
