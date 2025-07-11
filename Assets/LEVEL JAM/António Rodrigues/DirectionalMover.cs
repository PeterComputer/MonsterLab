using UnityEngine;

public class DirectionalMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveAmount = 1f;
    [SerializeField] private float moveDuration = 0.25f;

    [Header("Raycast Settings")]
    [SerializeField] private float raycastPadding = 0.1f;
    [SerializeField] private float raycastYOffset = 0.5f;

    [Header("Blocked Movement Shake")]
    [SerializeField] private float shakeStrength = 0.1f;
    [SerializeField] private float shakeDuration = 0.1f;

    private bool isMoving = false;

    private void Update()
    {
        DrawDebugRays();
    }

    public void MoveXPositive() => TryMove(Vector3.right);
    public void MoveXNegative() => TryMove(Vector3.left);
    public void MoveZPositive() => TryMove(Vector3.forward);
    public void MoveZNegative() => TryMove(Vector3.back);

    private void TryMove(Vector3 direction)
    {
        if (isMoving) return;

        Vector3 origin = transform.position + Vector3.up * raycastYOffset;
        float rayLength = moveAmount + raycastPadding;

        if (Physics.Raycast(origin, direction, rayLength))
        {
            StartCoroutine(Shake(direction));
        }
        else
        {
            StartCoroutine(MoveOverTime(direction));
        }
    }

    private System.Collections.IEnumerator MoveOverTime(Vector3 direction)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + direction * moveAmount;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    private System.Collections.IEnumerator Shake(Vector3 direction)
    {
        isMoving = true;

        Vector3 originalPos = transform.position;
        Vector3 shakeOffset = direction * -shakeStrength;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            transform.position = originalPos + shakeOffset * Mathf.Sin(elapsed / shakeDuration * Mathf.PI);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        isMoving = false;
    }

    private void DrawDebugRays()
    {
        Vector3 origin = transform.position + Vector3.up * raycastYOffset;
        float rayLength = moveAmount + raycastPadding;

        DrawRay(origin, Vector3.right, rayLength);
        DrawRay(origin, Vector3.left, rayLength);
        DrawRay(origin, Vector3.forward, rayLength);
        DrawRay(origin, Vector3.back, rayLength);
    }

    private void DrawRay(Vector3 origin, Vector3 direction, float length)
    {
        bool hit = Physics.Raycast(origin, direction, length);
        Color rayColor = hit ? Color.red : Color.green;
        Debug.DrawRay(origin, direction * length, rayColor);
    }
}
