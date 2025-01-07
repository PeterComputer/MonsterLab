using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]private int pickupsLeft;
    public Material openDoorMaterial;
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

    public void decreasePickupsLeft() {

        if (pickupsLeft > 0) {
            pickupsLeft--;
        }

        if(pickupsLeft == 0) {
            openDoor();
        }
    }

    private void openDoor() {
        gameObject.layer = 0;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        gameObject.GetComponent<MeshRenderer>().material = openDoorMaterial;
    }

    private void OnTriggerEnter(Collider other) {
        gameManager.loadNextScene();
        
    }
}
