using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLerping : MonoBehaviour
{
    public Vector3 minScale;
    public Vector3 maxScale;
    public bool repeatFlag;
    public float scalingSpeed;
    public float scalingDuration;

    private IEnumerator Start()
    {
        while (repeatFlag)
        {
            yield return RepeatLerping(minScale, maxScale, scalingDuration);
            yield return RepeatLerping(maxScale, minScale, scalingDuration);

        }
    }

    IEnumerator RepeatLerping(Vector3 startScale, Vector3 endScale, float time)
    {
        float t = 0.0f;
        float rate = (1f / time) * scalingSpeed;
        while (t < 1f)
        {
            t += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
    }
}