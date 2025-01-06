using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField]private int pickupsLeft;
    public Material openDoorMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
