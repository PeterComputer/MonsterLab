using UnityEngine;

public class DrawbridgeController : Obstacle
{
    private Vector3 targetAngle = new Vector3(0f, 0f, 0f);
    private bool isMoving;
    [SerializeField] private float rotationAmount;
    [SerializeField] private float rotationSpeed;
    private Collider bridgeCollider;
    private bool isLowered;
    private Vector3 currentAngle;

    void Awake()
    {
        bridgeCollider = GetComponentInChildren<Collider>();
    }
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
        targetAngle.z += rotationAmount;
    }

    public override void interactWith()
    {
        rotateXDegrees();

        // Return to previous state in the next interactWith()
        rotationAmount = -rotationAmount;
        isLowered = !isLowered;
        bridgeCollider.isTrigger = isLowered;
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
