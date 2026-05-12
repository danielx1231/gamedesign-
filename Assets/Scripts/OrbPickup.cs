using UnityEngine;

public class OrbPickup : MonoBehaviour
{
    private OrbFollow orb;

    void Start()
    {
        orb = GetComponent<OrbFollow>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ✅ 已放置就不能再拿
            if (orb.IsPlaced())
                return;

            orb.StartFollowing(other.transform);
        }
    }
}