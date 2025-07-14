using UnityEngine;

public class BridgeController : MonoBehaviour
{

    private Collider col;

    void Awake()
    {
        if (col == null)
        {
            col = GetComponent<Collider>();
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

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Collider>().layerOverridePriority = 0;
        }
    }    
}
