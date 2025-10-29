using UnityEngine;

public class disableObjectOnAnimEnd : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;
    public void OnAnimationFinished()
    {
        objectToDisable.SetActive(false);
    }
}
