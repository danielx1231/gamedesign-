using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI text;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private Color originalColor;
    private Color targetColor;

    public float scaleMultiplier = 1.6f;
    public float speed = 10f;

    public Color hoverColor = Color.white;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        originalColor = text.color;
        targetColor = originalColor; // ✅ 关键修复
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
        text.color = Color.Lerp(text.color, targetColor, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleMultiplier;
        targetColor = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        targetColor = originalColor;
    }
}