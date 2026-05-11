using UnityEngine;

public class ColorRuneStone : MonoBehaviour
{
    public enum RuneColor
    {
        None,
        Red,
        Yellow,
        Green
    }

    [Header("Visual")]
    public SpriteRenderer glowRenderer;

    [Header("Colors")]
    public Color redColor = new Color(1f, 0f, 0f, 1f);
    public Color yellowColor = new Color(1f, 0.85f, 0f, 1f);
    public Color greenColor = new Color(0f, 1f, 0f, 1f);
    public Color whiteColor = new Color(1f, 1f, 1f, 1f);

    [Header("Alpha")]
    public float offAlpha = 0f;
    public float onAlpha = 1f;

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Current State")]
    public RuneColor currentColor = RuneColor.None;

    private bool playerInRange = false;
    private bool isLocked = false;

    private void Awake()
    {
        if (glowRenderer == null)
        {
            Transform glow = transform.Find("Glow");

            if (glow != null)
            {
                glowRenderer = glow.GetComponent<SpriteRenderer>();
            }
        }

        currentColor = RuneColor.None;
        ApplyColor();
    }

    private void Start()
    {
        currentColor = RuneColor.None;
        ApplyColor();
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (isLocked) return;

        if (Input.GetKeyDown(interactKey))
        {
            CycleColor();
        }
    }

    public void CycleColor()
    {
        if (isLocked) return;

        if (currentColor == RuneColor.None)
        {
            currentColor = RuneColor.Red;
        }
        else if (currentColor == RuneColor.Red)
        {
            currentColor = RuneColor.Yellow;
        }
        else if (currentColor == RuneColor.Yellow)
        {
            currentColor = RuneColor.Green;
        }
        else if (currentColor == RuneColor.Green)
        {
            currentColor = RuneColor.Red;
        }

        ApplyColor();
    }

    public void ApplyColor()
    {
        if (glowRenderer == null)
        {
            Debug.LogWarning(gameObject.name + " has no Glow Renderer assigned.");
            return;
        }

        Color finalColor = Color.white;

        if (currentColor == RuneColor.None)
        {
            finalColor = whiteColor;
            finalColor.a = offAlpha;
        }
        else if (currentColor == RuneColor.Red)
        {
            finalColor = redColor;
            finalColor.a = onAlpha;
        }
        else if (currentColor == RuneColor.Yellow)
        {
            finalColor = yellowColor;
            finalColor.a = onAlpha;
        }
        else if (currentColor == RuneColor.Green)
        {
            finalColor = greenColor;
            finalColor.a = onAlpha;
        }

        glowRenderer.color = finalColor;
    }

    public void TurnOff()
    {
        if (isLocked) return;

        currentColor = RuneColor.None;
        ApplyColor();
    }

    public void LockRune()
    {
        isLocked = true;
    }

    public bool IsLit()
    {
        return currentColor != RuneColor.None;
    }

    public RuneColor GetCurrentColor()
    {
        return currentColor;
    }

    public void SetGlowVisible(bool visible)
    {
        if (glowRenderer == null) return;

        Color c = glowRenderer.color;
        c.a = visible ? onAlpha : 0f;
        glowRenderer.color = c;
    }

    public void SetGlowWhite(bool visible)
    {
        if (glowRenderer == null) return;

        Color c = whiteColor;
        c.a = visible ? onAlpha : 0f;
        glowRenderer.color = c;
    }

    public void RestoreCurrentColor()
    {
        ApplyColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}