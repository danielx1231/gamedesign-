using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            DeathRespawnBridge bridge = FindObjectOfType<DeathRespawnBridge>();

            if (bridge != null)
            {
                bridge.PrepareRespawnForDeath();
            }
            else
            {
                Debug.LogWarning("DeathRespawnBridge not found in scene.");
            }
        }
    }
}