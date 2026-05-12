using UnityEngine;

public class RuneStone : MonoBehaviour
{
    public Animator gateAnimator;
    public SpriteRenderer glow; // 🔥 直接拖 Glow 的 SpriteRenderer

    public float fadeSpeed = 3f;

    private bool playerInside = false;
    private bool orbPlaced = false;

    private float targetAlpha = 0f;
    private float currentAlpha = 0f;

    void Start()
    {
        gateAnimator.SetBool("isOpen", false);

        // 初始透明度 = 0
        SetAlpha(0f);
    }

    void Update()
    {
        // 🔥 平滑过渡透明度
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);
        SetAlpha(currentAlpha);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            UpdateVisual();
        }

        if (other.CompareTag("Orb"))
        {
            OrbFollow orb = other.GetComponent<OrbFollow>();

            if (orb != null && !orb.IsPlaced())
            {
                orb.PlaceOnStone(transform);

                orbPlaced = true;

                gateAnimator.SetBool("isOpen", true);

                UpdateVisual();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            UpdateVisual();
        }
    }

    void UpdateVisual()
    {
        // 🟣 orb 已放置 → 永久亮
        if (orbPlaced)
        {
            targetAlpha = 1f;
            return;
        }

        // 🟢 玩家在范围 → 渐亮
        if (playerInside)
        {
            targetAlpha = 1f;
        }
        else
        {
            // 🔴 玩家离开 → 渐暗
            targetAlpha = 0f;
        }

        gateAnimator.SetBool("isOpen", playerInside);
    }

    void SetAlpha(float a)
    {
        if (glow == null) return;

        Color c = glow.color;
        c.a = a;
        glow.color = c;
    }
}