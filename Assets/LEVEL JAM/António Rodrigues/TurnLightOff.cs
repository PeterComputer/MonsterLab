using UnityEngine;

public class TurnLightOff : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private float transitionSpeed = 1.0f;

    private float defaultIntensity;
    private float targetIntensity;
    private Coroutine currentTransition;

    private void Start()
    {
        if (directionalLight != null)
        {
            defaultIntensity = directionalLight.intensity;
            targetIntensity = defaultIntensity;
        }
        else
        {
            Debug.LogWarning("Directional Light is not assigned in the inspector.");
        }
    }

    public void ToggleLight()
    {
        if (directionalLight == null) return;

        // Toggle the target intensity
        targetIntensity = Mathf.Approximately(targetIntensity, 0f) ? defaultIntensity : 0f;

        // If there's already a transition running, stop it
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }

        // Start a new transition
        currentTransition = StartCoroutine(TransitionLightIntensity(targetIntensity));
    }

    private System.Collections.IEnumerator TransitionLightIntensity(float target)
    {
        while (!Mathf.Approximately(directionalLight.intensity, target))
        {
            directionalLight.intensity = Mathf.MoveTowards(directionalLight.intensity, target, transitionSpeed * Time.deltaTime);
            yield return null;
        }

        directionalLight.intensity = target;
        currentTransition = null;
    }
}
