using UnityEngine;

public class RotatingPlatformController : Obstacle
{
    public Vector3 targetAngle = new Vector3(0f, 0f, 0f);
    [SerializeField] private float rotationSpeed = 1f;

    private Vector3 currentAngle;

    public void Start()
    {
        currentAngle = transform.eulerAngles;
    }

    public void Update()
    {
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * rotationSpeed),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime * rotationSpeed),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime * rotationSpeed));

        transform.eulerAngles = currentAngle;
    }

    public void rotate90Degrees()
    {

        if (targetAngle.y == 270f)
        {
            targetAngle.y = 0f;
        }
        else
        {
            targetAngle.y += 90f;
        }

    }

    public void rotateToXDegrees(float angle, bool doAnimation)
    {
        targetAngle.y = angle;

        if (!doAnimation)
        {
            currentAngle = new Vector3(0f, angle, 0f);
        }
    }
    
    public override void interactWith()
    {
        rotate90Degrees();
    }    
}
