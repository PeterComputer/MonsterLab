using UnityEngine;
using Enums;
using System.Collections.Generic;
using System.Linq;

public class PartSelectorGrid : MonoBehaviour
{

    [SerializeField] private PickupType pickupType;
    private GameManager gameManager;
    [SerializeField] private List<PartButton> partButtonScripts;
    public GameObject selectButton;
    private bool hasBeenActivatedOnce;

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        selectButton.SetActive(false);
    }

    public void advanceToNextMission()
    {
        gameManager.advanceToNextMission(pickupType);
    }

    public void resetSelectedButton()
    {
        foreach (PartButton button in partButtonScripts)
        {
            button.setIsSelected(false);
        }

        if (!hasBeenActivatedOnce)
        {
            selectButton.SetActive(true);
            hasBeenActivatedOnce = true;
        }
    }    
}
