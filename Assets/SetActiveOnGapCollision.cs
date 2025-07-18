using UnityEngine;

public class SetActiveOnGapCollision : MonoBehaviour
{

    private Collider col;
    [SerializeField] private bool isEnteringAnotherGap;

    void Awake()
    {
        col = GetComponent<Collider>();
        isEnteringAnotherGap = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Gap")
        {
            col.isTrigger = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Gap")
        {
            isEnteringAnotherGap = true;
        }
        else
        {
            isEnteringAnotherGap = false;
        }

        Debug.Log("isEnteringAnotherGap = " + isEnteringAnotherGap + " for collision between " + this.gameObject.name + " and " + other.gameObject.name);

    }

    void OnCollisionExit(Collision other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Gap" && !isEnteringAnotherGap)
        {
            col.isTrigger = true;
        }
    }    

}
