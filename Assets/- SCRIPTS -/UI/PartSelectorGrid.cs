using UnityEngine;
using Enums;
using System.Collections.Generic;
using System.Linq;

public class PartSelectorGrid : MonoBehaviour
{

    [SerializeField] private PickupType pickupType;
    private GameManager gameManager;
    [SerializeField] private List<Animator> partButtons;
    public GameObject selectButton;
    private bool hasBeenActivatedOnce;

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        partButtons = gameObject.GetComponentsInChildren<Animator>().ToList();
        selectButton.SetActive(false);
    }

    public void advanceToNextMission()
    {
        gameManager.advanceToNextMission(pickupType);
    }

    public void resetSelectedButton()
    {
        foreach (Animator button in partButtons)
        {
            button.SetTrigger("TrIdle");
        }

        if (!hasBeenActivatedOnce)
        {
            selectButton.SetActive(true);
            hasBeenActivatedOnce = true;
        }
    }    
}
