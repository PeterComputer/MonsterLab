using UnityEngine;

public class MovingGateController : Obstacle
{
    [SerializeField][HideInInspector] public float moveAmount = 5f;
    [SerializeField][HideInInspector] private float moveSpeed = 5f;
    [SerializeField][HideInInspector] private Transform startPosition;
    [SerializeField][HideInInspector] private Transform endPosition;
    [SerializeField] private AudioClip activationAudioClip;

    private bool movingToEnd;
    private Vector3 currentPosition;

    public void Awake()
    {
        updateEndPosition();
    }

    public void Start()
    {
        currentPosition = transform.position;
    }

    public void Update()
    {

        if (movingToEnd)
        {
            currentPosition = new Vector3(
            Mathf.Lerp(currentPosition.x, endPosition.position.x, Time.deltaTime * moveSpeed),
            Mathf.Lerp(currentPosition.y, endPosition.position.y, Time.deltaTime * moveSpeed),
            Mathf.Lerp(currentPosition.z, endPosition.position.z, Time.deltaTime * moveSpeed));

        }

        else
        {
            currentPosition = new Vector3(
            Mathf.Lerp(currentPosition.x, startPosition.position.x, Time.deltaTime * moveSpeed),
            Mathf.Lerp(currentPosition.y, startPosition.position.y, Time.deltaTime * moveSpeed),
            Mathf.Lerp(currentPosition.z, startPosition.position.z, Time.deltaTime * moveSpeed));
        }

        transform.position = currentPosition;
    }

    public void movePosition()
    {
        movingToEnd = !movingToEnd;
    }

    public override void interactWith()
    {
        movePosition();

        if (activationAudioClip != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(activationAudioClip, transform, 1f);
        }  
    }

    // Moving only on the local x axis makes the door move left and right
    public void updateEndPosition()
    {
        endPosition.localPosition = new Vector3(moveAmount, 0, 0);
    }

    /*
    *   Set Functions
    */
    public void setMoveAmount(float newMoveAmount)
    {
        moveAmount = newMoveAmount;
        updateEndPosition();
    }

    public void setMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }
}
