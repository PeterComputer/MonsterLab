using UnityEngine;

public class TeleporterController : Obstacle
{
    // Teleporter Variables
    [SerializeField][HideInInspector] private TeleporterController pairedTeleporter;
    [SerializeField][HideInInspector] private Collider teleporterCollider;
    [SerializeField][HideInInspector] private float zTeleportOffset;
    private bool canTeleport = true;
    [SerializeField][HideInInspector] private bool isDoorOpen = true;
    [SerializeField] private AudioClip teleportAudioClip;
    [SerializeField] private AudioClip openDoorAudioClip;


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

    // Teleports a colliding gameObject to this teleporter's position
    public void TeleportTo(Collider other)
    {
        if (isDoorOpen)
        {
            other.gameObject.transform.position = new Vector3(transform.position.x, other.gameObject.transform.position.y, transform.position.z + zTeleportOffset);
            canTeleport = false;

            if (teleportAudioClip != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(teleportAudioClip, transform, 1f);
            }
        }
    }

    private void openTeleporterDoor()
    {
        if (!isDoorOpen)
        {
            setIsDoorOpen(true);

            if (openDoorAudioClip != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(openDoorAudioClip, transform, 1f);
            }
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
