using UnityEngine;

public class SetActiveOnGapCollision : MonoBehaviour
{

    [SerializeField] private Collider col;
    [SerializeField] private int touchingGapsCount;

    void Awake()
    {
    }

    // If a pass-through barrier enters a gap, make it solid (aka !isTrigger)
    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Gap")
        {
            touchingGapsCount++;
            setState();
        }
    }

    // If a solid barrier exits a gap, make it pass-through (aka isTrigger)
    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Gap")
        {
            if (--touchingGapsCount < 0) touchingGapsCount = 0;
            setState();
        }
    }

    private void setState()
    {
        if (touchingGapsCount > 0)
        {
            col.isTrigger = false;
        }
        else
        {
            col.isTrigger = true;
        }
    }
}