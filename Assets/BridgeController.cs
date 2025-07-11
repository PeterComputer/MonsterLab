using UnityEngine;

public class BridgeController : MonoBehaviour
{

    private Collider collider;

    void Awake()
    {
        if (collider == null)
        {
            collider = GetComponent<Collider>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Collider>().layerOverridePriority = 5;
        }
    }

    // Not working great, fix on monday
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {   

            // Checks if the player hitbox is fully inside the bridge collider
            if (collider.bounds.Contains(other.bounds.max) && collider.bounds.Contains(other.bounds.min))
            {
                Debug.Log("Fully contained");
                other.gameObject.GetComponent<Collider>().layerOverridePriority = 5;
            }
            else
            {
                Debug.Log("Partially contained");
                other.gameObject.GetComponent<Collider>().layerOverridePriority = 0;
            }
        }        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Collider>().layerOverridePriority = 0;
        }
    }    
}
