using UnityEngine;
using Enums;

public class PickupController : MonoBehaviour
{

    [SerializeField]
    private PickupType type;
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
        gameManager.openSelectionUI(type);
    }

    public PickupType getPickupType() {
        return type;
    }
}
