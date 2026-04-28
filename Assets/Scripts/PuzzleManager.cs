using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public int[] correctOrder = { 0, 1, 2, 3 };

    public GameObject runeStone; // 成功后触发动画用
    private List<int> playerInput = new List<int>();
    private bool isFinished = false;

    // Button 只需要调用这个，不要再传 callback
    public void OnButtonPress(int buttonIndex)
    {
        if (isFinished) return;

        int expectedIndex = playerInput.Count;
        if (expectedIndex >= correctOrder.Length) return;

        if (buttonIndex == correctOrder[expectedIndex])
        {
            playerInput.Add(buttonIndex);

            if (playerInput.Count == correctOrder.Length)
            {
                Success();
            }
        }
        else
        {
            ResetPuzzle();
        }
    }

    private void ResetPuzzle()
    {
        playerInput.Clear();

        // 全部渐隐并允许重新按（按钮脚本里已经把 isLit 设回 false）
        PuzzleButton[] allButtons = FindObjectsOfType<PuzzleButton>();
        foreach (var btn in allButtons)
        {
            btn.TurnOffGlow();
        }
    }

    private void Success()
    {
        isFinished = true;
        Debug.Log("解密成功！执行闪烁效果");

        PuzzleButton[] allButtons = FindObjectsOfType<PuzzleButton>();
        foreach (var btn in allButtons) btn.LockAndFlash();

        // ✅ 切换BGM（淡出→换歌→淡入）
        if (BGMController.Instance == null)
            Debug.LogError("BGMController.Instance is NULL! Make sure BGMController exists in the scene.");
        else
        {
            Debug.Log("Calling SwitchToAfterPuzzleBGM()");
            BGMController.Instance.SwitchToAfterPuzzleBGM();
        }

        if (runeStone != null)
        {
            Animator anim = runeStone.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Active");
        }
    }
}