using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public FloatPlatform floatingPlatform;
    public int[] correctOrder = { 0, 1, 2, 3 };

    [Header("Success Objects")]
    public GameObject runeStone;

    [Header("Countdown System")]
    public CountdownChallengeManager countdownManager;

    private List<int> playerInput = new List<int>();
    private bool isFinished = false;

    private void Start()
    {
        // 如果之前已经解锁过倒计时挑战，说明这是从 checkpoint 复活/重载场景
        // 这里恢复谜题完成后的机关状态，但不再次弹出提示窗口
        if (CountdownCheckpointData.challengeUnlocked)
        {
            RestoreSolvedState();
        }
    }

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

        PuzzleButton[] allButtons = FindObjectsOfType<PuzzleButton>();

        foreach (var btn in allButtons)
        {
            btn.TurnOffGlow();
        }
    }

    private void Success()
    {
        isFinished = true;
        Debug.Log("Puzzle solved.");

        // 保存：这个挑战已经被解锁
        CountdownCheckpointData.UnlockChallenge();

        PuzzleButton[] allButtons = FindObjectsOfType<PuzzleButton>();

        foreach (var btn in allButtons)
        {
            btn.LockAndFlash();
        }

        ActivateSolvedMechanisms();

        if (countdownManager != null)
        {
            countdownManager.ShowCountdownPopup();
        }
        else
        {
            Debug.LogWarning("CountdownChallengeManager is not assigned in PuzzleManager.");
        }
    }

    private void RestoreSolvedState()
    {
        isFinished = true;
        playerInput.Clear();

        Debug.Log("PuzzleManager restored solved state.");

        PuzzleButton[] allButtons = FindObjectsOfType<PuzzleButton>();

        foreach (var btn in allButtons)
        {
            btn.LockAndFlash();
        }

        ActivateSolvedMechanisms();
    }

    private void ActivateSolvedMechanisms()
    {
        if (runeStone != null)
        {
            Animator anim = runeStone.GetComponent<Animator>();

            if (anim != null)
            {
                anim.SetTrigger("Active");
            }
        }

        if (floatingPlatform != null)
        {
            floatingPlatform.Activate();
        }
    }
}