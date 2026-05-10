using UnityEngine;

public class DeathRespawnBridge : MonoBehaviour
{
    public void PrepareRespawnForDeath()
    {
        PrepareRespawnForDeathStatic();
    }

    public static void PrepareRespawnForDeathStatic()
    {
        Time.timeScale = 1f;

        if (CountdownCheckpointData.hasCheckpoint)
        {
            CountdownCheckpointData.PrepareRespawn();
            Debug.Log("Death respawn prepared at checkpoint.");
        }
        else
        {
            Debug.Log("No checkpoint found. Respawn from level start.");
        }
    }
}