using UnityEngine;

public class DrawbridgeController : Obstacle
{
    private Vector3 targetAngle = new Vector3(0f, 0f, 0f);
    [SerializeField][HideInInspector] private float rotationAmount;
    [SerializeField][HideInInspector] private float rotationSpeed;
    [SerializeField][HideInInspector] private Collider bridgeCollider;
    private bool isLowered;
    private Vector3 currentAngle;
    [SerializeField][HideInInspector] private SpriteRenderer bridge;
    [SerializeField][HideInInspector] private SpriteRenderer[] railings;
    [SerializeField] private AudioClip activationAudioClip;   

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

        if (activationAudioClip != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(activationAudioClip, transform, 1f);
        }
    }

    /*
    *   Set Functions
    */
    public void setRotationAmount(float amount)
    {   
        // Keep the original rotationAmount's sign (needed if you're setting a new value during play mode)
        rotationAmount = Mathf.Sign(rotationAmount) * amount;
    }

    public void setRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }

    public void changeDrawbridgeColor(Material newBridgeColor, Material newRailingColor)
    {
        bridge.material = newBridgeColor;

        foreach (SpriteRenderer railing in railings)
        {
            railing.material = newRailingColor;
        }
    }
}
