using UnityEngine;

public class ResetBoxWhenFullyOffscreen : MonoBehaviour
{
    public Camera targetCam;          // 不填就用 Camera.main
    public float margin = 0.02f;      // 给一点容错，避免贴边抖动（可改 0）

    private Vector3 startPos;
    private Rigidbody2D rb;
    private Renderer rend;            // SpriteRenderer 也算 Renderer

    void Awake()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<Renderer>(); // 有子物体也能拿到
        if (targetCam == null) targetCam = Camera.main;
    }

    void LateUpdate()
    {
        if (targetCam == null || rend == null) return;

        Bounds b = rend.bounds;

        // 取包围盒四个角（世界坐标）
        Vector3 p1 = targetCam.WorldToViewportPoint(new Vector3(b.min.x, b.min.y, b.center.z));
        Vector3 p2 = targetCam.WorldToViewportPoint(new Vector3(b.min.x, b.max.y, b.center.z));
        Vector3 p3 = targetCam.WorldToViewportPoint(new Vector3(b.max.x, b.min.y, b.center.z));
        Vector3 p4 = targetCam.WorldToViewportPoint(new Vector3(b.max.x, b.max.y, b.center.z));

        // 如果在摄像机背后，也算看不到
        if (p1.z < 0f && p2.z < 0f && p3.z < 0f && p4.z < 0f)
        {
            ResetBox();
            return;
        }

        // “完全不可见”的判定：四个角都在同一侧屏幕外
        bool allLeft = p1.x < 0f - margin && p2.x < 0f - margin && p3.x < 0f - margin && p4.x < 0f - margin;
        bool allRight = p1.x > 1f + margin && p2.x > 1f + margin && p3.x > 1f + margin && p4.x > 1f + margin;
        bool allBelow = p1.y < 0f - margin && p2.y < 0f - margin && p3.y < 0f - margin && p4.y < 0f - margin;
        bool allAbove = p1.y > 1f + margin && p2.y > 1f + margin && p3.y > 1f + margin && p4.y > 1f + margin;

        if (allLeft || allRight || allBelow || allAbove)
            ResetBox();
    }

    private void ResetBox()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        transform.position = startPos;
    }
}