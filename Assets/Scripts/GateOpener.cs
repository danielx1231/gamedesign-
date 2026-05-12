using UnityEngine;

public class GateOpener : MonoBehaviour
{
    [Header("引用设置")]
    public Animator switchAnimator;
    public Animator gateAnimator;

    [Header("玩家检测")]
    public string playerTag = "Player";

    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerMechanic();
        }
    }

    private void TriggerMechanic()
    {
        Debug.Log("GateOpener triggered");

        if (switchAnimator != null)
        {
            switchAnimator.SetTrigger("Open");
        }

        if (gateAnimator != null)
        {
            gateAnimator.SetTrigger("Open");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;
            Debug.Log("靠近开关，按 E 交互");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
        }
    }
}