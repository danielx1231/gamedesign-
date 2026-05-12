using UnityEngine;

public class RoomEnterDoorTrigger : MonoBehaviour
{
    public PuzzleDoorController door;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (door != null)
            {
                door.CloseDoor();
            }
            else
            {
                Debug.LogWarning("Door is not assigned on RoomEnterDoorTrigger.");
            }
        }
    }
}