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

        PuzzleButton[] allButtons = FindObjectsOfType<PuzzleButton>();

        foreach (var btn in allButtons)
        {
            btn.LockAndFlash();
        }

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

        if (countdownManager != null)
        {
            countdownManager.ShowCountdownPopup();
        }
        else
        {
            Debug.LogWarning("CountdownChallengeManager is not assigned in PuzzleManager.");
        }
    }
}