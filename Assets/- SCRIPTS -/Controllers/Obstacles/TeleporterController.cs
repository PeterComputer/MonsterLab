using UnityEngine;

public class TeleporterController : Obstacle
{
    // Teleporter Variables
    [SerializeField][HideInInspector] private TeleporterController pairedTeleporter;
    [SerializeField][HideInInspector] private Collider teleporterCollider;
    private bool canTeleport = true;
    [SerializeField][HideInInspector] private bool isDoorOpen = true;


    // Teleporter Sprites
    [SerializeField][HideInInspector] private SpriteRenderer teleportPortal;
    [SerializeField][HideInInspector] private SpriteRenderer teleportDoor;
    [SerializeField][HideInInspector] private SpriteRenderer teleportFrame;

    private void OnTriggerEnter(Collider other)
    {
        if (canTeleport)
        {
            pairedTeleporter.TeleportTo(other);
        }

    }

    private void OnTriggerExit()
    {
        canTeleport = true;
    }

    public override void interactWith()
    {
        openTeleporterDoor();
    }

    public void TeleportTo(Collider other)
    {
        if (isDoorOpen)
        {
            other.gameObject.transform.position = new Vector3(transform.position.x, other.gameObject.transform.position.y, transform.position.z);
            canTeleport = false;            
        }

    }

    private void openTeleporterDoor()
    {
        if (!isDoorOpen)
        {
            setIsDoorOpen(true);
        }
    }

    public void changeTeleporterColor(Material newDetailColor, Material newPortalColor)
    {
        teleportPortal.material = newPortalColor;
        teleportDoor.material = newDetailColor;
        teleportFrame.material = newDetailColor; 
    }


    // Get / Set Methods

    public bool getIsDoorOpen()
    {
        return isDoorOpen;
    }

    public void setIsDoorOpen(bool newIsDoorOpen)
    {
        isDoorOpen = newIsDoorOpen;

        teleportDoor.enabled = !isDoorOpen;
        teleporterCollider.isTrigger = isDoorOpen;
    }

    public TeleporterController getPairedTeleporter()
    {
        return pairedTeleporter;
    }

    public void setPairedTeleporter(TeleporterController newPairedTeleporter)
    {
        pairedTeleporter = newPairedTeleporter;
    }
}
