using UnityEngine;

public class TeleporterController : Obstacle
{

    [SerializeField] private TeleporterController pairedTeleporter;
    private bool canTeleport = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
        //
    }

    public void TeleportTo(Collider other)
    {
        other.gameObject.transform.position = new Vector3(transform.position.x, other.gameObject.transform.position.y, transform.position.z);
        canTeleport = false;
    }
}
