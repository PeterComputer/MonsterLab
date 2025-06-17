using UnityEngine;

public class MovingGateController : Obstacle
{
    public Vector3 targetPosition;
    [SerializeField] public float moveAmount = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int movesOnAxis = 0;

    private Vector3 currentPosition;

    public void Start()
    {
        currentPosition = transform.position;
        targetPosition = transform.position;
    }

    public void Update()
    {
        currentPosition = new Vector3(
            Mathf.Lerp(currentPosition.x, targetPosition.x, Time.deltaTime * moveSpeed),
            Mathf.Lerp(currentPosition.y, targetPosition.y, Time.deltaTime * moveSpeed),
            Mathf.Lerp(currentPosition.z, targetPosition.z, Time.deltaTime * moveSpeed));

        transform.position = currentPosition;
    }

    public void movePosition()
    {

        switch (movesOnAxis)
        {
            //move on x axis
            case 0:
                targetPosition.x += moveAmount;
                break;
            //move on y axis
            case 1:
                targetPosition.y += moveAmount;
                break;
            //move on z axis
            case 2:
                targetPosition.z += moveAmount;
                break;
        }

        moveAmount = -moveAmount;
    }

    public override void interactWith()
    {
        movePosition();
    }
}
