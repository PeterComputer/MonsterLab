using UnityEngine;

public class RotatingPlatformController : Obstacle
{
    private Vector3 targetAngle = new Vector3(0f, 0f, 0f);
    [SerializeField] [HideInInspector] private float rotationAmount;
    [SerializeField] [HideInInspector] private float rotationSpeed;

    private Vector3 currentAngle;

    public void Start()
    {
        currentAngle = transform.eulerAngles;
        targetAngle = currentAngle;
    }

    public void Update()
    {
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * rotationSpeed),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime * rotationSpeed),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime * rotationSpeed));

        transform.eulerAngles = currentAngle;
    }

    public void rotateXDegrees()
    {
        targetAngle.y += rotationAmount;

        // Just here to ensure target angle stays between 0 and 360, for ease of reading during debugging
        // Breaks if rotationAmount is bigger/smaller than 360/-360, so don't do that please
        if (targetAngle.y >= 360f) targetAngle.y -= 360f;
        if (targetAngle.y <= -360f) targetAngle.y += 360f;
    }

    public override void interactWith()
    {
        rotateXDegrees();
    }

    /*
    *   Set Functions
    */
    public void setRotationAmount(float amount)
    {
        rotationAmount = amount;
    }

    public void setRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }
}
