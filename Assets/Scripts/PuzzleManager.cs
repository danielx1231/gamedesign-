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
        Debug.Log("Puzzle solved! Flash all stones.");

        // 全部锁定并闪烁 4 次
        PuzzleButton[] allButtons = FindObjectsOfType<PuzzleButton>();
        foreach (var btn in allButtons)
        {
            btn.LockAndFlash();
        }

        // 触发 runeStone 动画
        if (runeStone != null)
        {
            Animator anim = runeStone.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Active");
        }
    }
}