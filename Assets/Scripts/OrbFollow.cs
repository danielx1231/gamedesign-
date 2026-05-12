using UnityEngine;

public class OrbFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    public float floatAmplitude = 0.2f;
    public float floatSpeed = 2f;

    private bool isFollowing = false;
    private bool isPlaced = false;
    private bool isMovingToStone = false;

    private Vector3 basePosition;
    private Vector3 targetPosition;

    void Start()
    {
        basePosition = transform.position;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
    }

    void Update()
    {
        // ✅ 只保留这一处 yOffset
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // 🟢 跟随玩家
        if (isFollowing && target != null && !isPlaced)
        {
            basePosition = target.position + offset;

            transform.position = Vector3.Lerp(
                transform.position,
                basePosition + new Vector3(0, yOffset, 0),
                Time.deltaTime * followSpeed
            );
        }

        // 🔵 飞向符文石（平滑）
        if (isMovingToStone)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                followSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMovingToStone = false;
                isPlaced = true;
                basePosition = targetPosition;
            }
        }

        // 🟣 放置后漂浮
        else if (isPlaced)
        {
            transform.position = basePosition + new Vector3(0, yOffset, 0);
        }

        // 🟡 初始漂浮
        else if (!isFollowing)
        {
            transform.position = basePosition + new Vector3(0, yOffset, 0);
        }
    }

    // ✅ 玩家拾取
    public void StartFollowing(Transform player)
    {
        target = player;
        isFollowing = true;
        isPlaced = false;
    }

    // ✅ 放置到符文石
    public void PlaceOnStone(Transform stone)
    {
        isFollowing = false;

        targetPosition = stone.position + new Vector3(0, 1.5f, 0);
        isMovingToStone = true;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}