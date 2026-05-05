using UnityEngine;

public class ClockPickup : MonoBehaviour
{
    [Header("Clock Settings")]
    public float bonusTime = 25f;

    [Header("Floating Animation")]
    public float floatAmplitude = 0.2f; // 上下浮动幅度
    public float floatSpeed = 2f;       // 浮动速度

    private bool collected = false;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (collected) return;

        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = startPosition + new Vector3(0f, yOffset, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            collected = true;

            CountdownChallengeManager timer = FindObjectOfType<CountdownChallengeManager>();

            if (timer != null)
            {
                timer.AddTime(bonusTime);
            }
            else
            {
                Debug.LogWarning("CountdownChallengeManager not found in scene.");
            }

            Destroy(gameObject);
        }
    }
}