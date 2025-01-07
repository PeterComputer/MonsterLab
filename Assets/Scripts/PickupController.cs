using UnityEngine;
using Enums;

public class PickupController : MonoBehaviour
{

    [SerializeField]
    private PickupType type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other) {
        GameObject.FindWithTag("Door").GetComponent<DoorController>().decreasePickupsLeft();
        Destroy(gameObject);
    }
}
