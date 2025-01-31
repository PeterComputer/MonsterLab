using UnityEngine;

public class InteractibleArea : MonoBehaviour
{

    public GameObject[] connectedObjects;

    private void OnTriggerEnter() {
        foreach(GameObject currentObject in connectedObjects) {
            if(currentObject != null)
            currentObject.SetActive(!currentObject.activeSelf);
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
}
