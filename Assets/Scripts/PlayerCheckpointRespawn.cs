using UnityEngine;

public class PlayerCheckpointRespawn : MonoBehaviour
{
    void Start()
    {
        if (CountdownCheckpointData.shouldRespawnAtCheckpoint && CountdownCheckpointData.hasCheckpoint)
        {
            transform.position = CountdownCheckpointData.checkpointPosition;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            CountdownCheckpointData.shouldRespawnAtCheckpoint = false;
        }
    }
}