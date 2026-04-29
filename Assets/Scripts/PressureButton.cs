using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public Animator buttonAnim;
    public Animator platformAnim;

    public string buttonParam = "isPressed";
    public string platformParam = "isOpen";

    public string triggerLayerName = "Tigger";
    private int pressCount = 0;

    private bool IsValid(Collider2D other)
    {
        return other.gameObject.layer == LayerMask.NameToLayer(triggerLayerName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsValid(other)) return;
        pressCount++;
        SetState(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsValid(other)) return;
        pressCount = Mathf.Max(0, pressCount - 1);
        if (pressCount == 0) SetState(false);
    }

    private void SetState(bool pressed)
    {
        if (buttonAnim) buttonAnim.SetBool(buttonParam, pressed);
        if (platformAnim) platformAnim.SetBool(platformParam, pressed);
    }
}