using UnityEngine;

public class SignPuzzleClue : MonoBehaviour
{
    private AudioSource audioSource; // 引用自身物体上的 AudioSource 组件
    private bool isPlayerInRange = false; // 玩家是否在交互范围内

    void Start()
    {
        // 获取当前物体上的 AudioSource 组件
        audioSource = GetComponent<AudioSource>();

        // 简单错误检查
        if (audioSource == null)
        {
            Debug.LogError("物体 " + gameObject.name + " 上缺少 AudioSource 组件！");
        }
    }

    void Update()
    {
        // 只有当玩家在范围内，且按下 E 键时触发
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayClueMusic();
        }
    }

    private void PlayClueMusic()
    {
        if (audioSource != null && !audioSource.isPlaying) // 如果没有正在播放，则播放
        {
            audioSource.Play();
            Debug.Log("播放解密线索音乐");
        }
    }

    // 碰撞检测：玩家进入范围
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 确认碰到的是玩家（检查 Tag 为 "Player"）
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("按 E 听取线索音乐");
        }
    }

    // 碰撞检测：玩家离开范围
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // (可选) 离开范围时停止播放，防止玩家跑远了还在听
            // if (audioSource != null) audioSource.Stop();
        }
    }
}