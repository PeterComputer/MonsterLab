using UnityEngine;

public class TeleporterController : Obstacle
{
    // Teleporter Variables
    [SerializeField] private TeleporterController pairedTeleporter;
    [SerializeField] private Collider teleporterCollider;
    private bool canTeleport = true;
    private bool isDoorOpen = true;


    // Teleporter Sprites
    [SerializeField] private SpriteRenderer teleportPortal;
    [SerializeField] private SpriteRenderer teleportDoor;
    [SerializeField] private SpriteRenderer teleportFrame;

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
        other.gameObject.transform.position = new Vector3(transform.position.x, other.gameObject.transform.position.y, transform.position.z);
        canTeleport = false;
    }

    private void openTeleporterDoor()
    {
        if (!isDoorOpen)
        {
            setIsDoorOpen(true);
        }
    }

    public void changeTeleporterColor(Color newDetailColor, Color newPortalColor)
    {
        teleportPortal.color = newPortalColor;
        teleportDoor.color = newDetailColor;
        teleportFrame.color = newDetailColor; 
    }


    // Get / Set Methods

    private bool getIsDoorOpen()
    {
        return isDoorOpen;
    }

    private void setIsDoorOpen(bool newIsDoorOpen)
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
