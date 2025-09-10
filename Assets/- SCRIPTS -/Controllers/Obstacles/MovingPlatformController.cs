using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : Obstacle
{

    [SerializeField][HideInInspector] public float moveAmount = 5f;
    [SerializeField][HideInInspector] private float moveSpeed = 5f;
    [SerializeField][HideInInspector] private List<Transform> platformPositionsList;
    private int positionIndex;
    private Vector3 currentPosition;
    [SerializeField][HideInInspector] private Transform endPosition;
    [SerializeField][HideInInspector] private SpriteRenderer bridge;
    [SerializeField][HideInInspector] private SpriteRenderer[] railings;
    [SerializeField] private AudioClip activationAudioClip;
    private bool goingForward;

    public void Awake()
    {
        positionIndex = 0;
        endPosition = platformPositionsList[0];
        goingForward = true;
    }

    public void Start()
    {
        currentPosition = transform.position;
    }

    public void Update()
    {
        currentPosition = new Vector3(
        Mathf.Lerp(currentPosition.x, endPosition.position.x, Time.deltaTime * moveSpeed),
        Mathf.Lerp(currentPosition.y, endPosition.position.y, Time.deltaTime * moveSpeed),
        Mathf.Lerp(currentPosition.z, endPosition.position.z, Time.deltaTime * moveSpeed));

        transform.position = currentPosition;
    }

    public void movePosition()
    {
        // If the platform is traversing the positions forward
        if (goingForward)
        {
            positionIndex++;

            if (positionIndex == platformPositionsList.Count - 1)
            {
                goingForward = false;
            }
        }

        // If the platform is traversing the positions backwards
        else
        {
            positionIndex--;
            
            if (positionIndex == 0)
            {
                goingForward = true;
            }            
        }

        // Finally, set current position's value to endPosition
        endPosition = platformPositionsList[positionIndex];
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


    public void changeMovingPlatformColor(Material newBridgeColor, Material newRailingColor)
    {
        bridge.material = newBridgeColor;

        foreach (SpriteRenderer railing in railings)
        {
            railing.material = newRailingColor;
        }
    }

    /*
    *   Set Functions
    */
    public void setMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public void setPlatformPositions(List<Transform> newPlatformPositions)
    {
        platformPositionsList = newPlatformPositions;

/*         LinkedListNode<Transform> tempNode = platformPositionsLinkedList.First;

        foreach (Transform newTransform in newPlatformPositions)
        {
            // Expands the list if needed
            if (tempNode == null)
            {
                newPlatformPositions.Add(newTransform);
            }

            // Replaces values of nodes in list
            else
            {
                tempNode.Value = newTransform;
                tempNode = tempNode.Next;
            }
        }

        // Then checks whether there are old values still present in the LinkedList
        int sizeDifference = platformPositionsLinkedList.Count - newPlatformPositions.Count;

        if (sizeDifference > 0)
        {
            // Deletes old values, if present
            for (int i = 0; i < sizeDifference; i++)
            {
                platformPositionsLinkedList.Remove(platformPositionsLinkedList.Last);
            }
        } */

    }
}
