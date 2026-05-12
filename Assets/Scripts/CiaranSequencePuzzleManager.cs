using System.Collections.Generic;
using UnityEngine;

public class CiaranSequencePuzzleManager : MonoBehaviour
{
    [Header("Correct Sequence: 3, 1, 4, 1, 5")]
    public int[] correctSequence = { 3, 1, 4, 1, 5 };

    [Header("Levers")]
    public CiaranSequenceLever[] levers;

    [Header("Lever 7")]
    public GameObject lever7ToShow;

    private readonly List<int> currentSequence = new List<int>();
    private bool puzzleSolved = false;

    private void Start()
    {
        if (lever7ToShow != null)
        {
            lever7ToShow.SetActive(false);
        }
    }

    public void RegisterLeverInput(int leverNumber)
    {
        if (puzzleSolved) return;

        currentSequence.Add(leverNumber);

        Debug.Log("Current sequence: " + string.Join(",", currentSequence));

        int index = currentSequence.Count - 1;

        if (currentSequence[index] != correctSequence[index])
        {
            Debug.Log("Wrong sequence. Reset.");
            ResetPuzzle();
            return;
        }

        if (currentSequence.Count == correctSequence.Length)
        {
            puzzleSolved = true;
            Debug.Log("Correct sequence 31415. Lever 7 appeared.");

            if (lever7ToShow != null)
            {
                lever7ToShow.SetActive(true);
            }
        }
    }

    public void ResetPuzzle()
    {
        currentSequence.Clear();
        puzzleSolved = false;

        foreach (CiaranSequenceLever lever in levers)
        {
            if (lever != null)
            {
                lever.ResetLeverState();
            }
        }

        if (lever7ToShow != null)
        {
            lever7ToShow.SetActive(false);
        }

        Debug.Log("Puzzle reset.");
    }
}