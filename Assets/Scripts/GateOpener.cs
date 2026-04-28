using UnityEngine;

public class GateOpener : MonoBehaviour
{
    [Header("引用设置")]
    public Animator switchAnimator; // 开关的 Animator
    public Animator gateAnimator;   // 铁门的 Animator

    private bool isPlayerInRange = false; // 玩家是否在开关附近

    void Update()
    {
        // 只有当玩家在范围内，且按下 E 键时触发
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerMechanic();
        }
    }

    private void TriggerMechanic()
    {
        // 触发开关动画
        if (switchAnimator != null)
        {
            switchAnimator.SetTrigger("Open");
        }

        // 触发铁门动画
        if (gateAnimator != null)
        {
            gateAnimator.SetTrigger("Open");
        }

        // 交互后禁用脚本，防止多次触发（如果只需要开一次门）
        // this.enabled = false; 
    }

    // 碰撞检测：玩家进入
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "player") // 匹配你的角色名
        {
            isPlayerInRange = true;
            Debug.Log("靠近开关，按 E 交互");
        }
    }

    // 碰撞检测：玩家离开
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "player")
        {
            isPlayerInRange = false;
        }
    }
}