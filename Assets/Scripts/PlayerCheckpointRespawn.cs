using UnityEngine;

public class PlayerCheckpointRespawn : MonoBehaviour
{
    private const string HasCheckpointKey = "HasCountdownCheckpoint";
    private const string CheckpointXKey = "CountdownCheckpointX";
    private const string CheckpointYKey = "CountdownCheckpointY";
    private const string CheckpointZKey = "CountdownCheckpointZ";

    void Start()
    {
        if (PlayerPrefs.GetInt(HasCheckpointKey, 0) == 1)
        {
            float x = PlayerPrefs.GetFloat(CheckpointXKey);
            float y = PlayerPrefs.GetFloat(CheckpointYKey);
            float z = PlayerPrefs.GetFloat(CheckpointZKey);

            transform.position = new Vector3(x, y, z);

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}