using UnityEngine;

public class InteractibleArea : MonoBehaviour
{

    public GameObject connectedObject;

    private void OnTriggerEnter() {
        Debug.Log("triggered");
        connectedObject.SetActive(!connectedObject.activeSelf);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
