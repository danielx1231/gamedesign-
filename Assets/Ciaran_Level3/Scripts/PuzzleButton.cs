using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PuzzleButton : MonoBehaviour
{
    public int myID;
    public SpriteRenderer glowSprite;
    public AudioClip noteSound;

    private AudioSource audioSource;
    private PuzzleManager manager;

    private bool isPlayerInBox = false;
    private bool canInteract = true;
    private bool isLit = false;

    private Coroutine fadeCoroutine;
    private Coroutine flashCoroutine;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        manager = FindObjectOfType<PuzzleManager>();

        // 防止忘记拖引用：自动找子物体 SpriteRenderer（你Glow是子物体的话也能找到）
        if (glowSprite == null)
            glowSprite = GetComponentInChildren<SpriteRenderer>(true);

        ForceGlowOffImmediate();
    }

    void OnEnable()
    {
        // 场景重载/对象重新启用时确保不亮
        ForceGlowOffImmediate();
        canInteract = true;
        isLit = false;
    }

    void Update()
    {
        if (!canInteract) return;

        if (isPlayerInBox && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        // 防止重复按：如果已经亮了，就不再重复触发（按错会被Manager统一熄灭）
        if (isLit) return;

        isLit = true;

        if (noteSound != null) audioSource.PlayOneShot(noteSound);

        // 缓慢亮起
        StartFade(1f, 0.25f);

        if (manager != null)
        {
            manager.OnButtonPress(myID);
        }
    }

    // Manager 调用：按错时全部渐隐并可再次按
    public void TurnOffGlow()
    {
        isLit = false;
        StartFade(0f, 0.35f);
    }

    // Manager 调用：成功后锁定并闪烁 4 次
    public void LockAndFlash()
    {
        canInteract = false;
        isLit = true;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashSequence(4));
    }

    private IEnumerator FlashSequence(int times)
    {
        // 用渐变闪烁更柔和
        for (int i = 0; i < times; i++)
        {
            yield return FadeRoutine(0f, 0.12f);
            yield return FadeRoutine(1f, 0.12f);
        }
        SetGlowAlpha(1f); // 最后保持亮
    }

    private void StartFade(float targetAlpha, float duration)
    {
        if (glowSprite == null) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha, duration));
    }

    private IEnumerator FadeRoutine(float target, float duration)
    {
        if (glowSprite == null) yield break;

        float start = glowSprite.color.a;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, target, t / duration);
            SetGlowAlpha(a);
            yield return null;
        }
        SetGlowAlpha(target);
    }

    private void ForceGlowOffImmediate()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        SetGlowAlpha(0f);
    }

    private void SetGlowAlpha(float alpha)
    {
        if (glowSprite == null) return;
        var c = glowSprite.color;
        c.a = alpha;
        glowSprite.color = c;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInBox = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInBox = false;
    }
}