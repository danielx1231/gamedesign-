using UnityEngine;
using System.Collections;

public class ColorRunePuzzleTrigger : MonoBehaviour
{
    [Header("Puzzle Manager")]
    public ColorRunePuzzleManager puzzleManager;

    [Header("Switch Animation")]
    public Animator switchAnimator;
    public string switchOnStateName = "Switch on";
    public string switchOffStateName = "Switch off";

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Timing")]
    public float switchOffDelay = 0.5f;

    private bool playerInRange = false;
    private bool isInteracting = false;
    private bool isSolved = false;

    private void Awake()
    {
        if (switchAnimator == null)
        {
            switchAnimator = GetComponent<Animator>();
        }

        PlaySwitchOff();
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (isInteracting) return;
        if (isSolved) return;

        if (Input.GetKeyDown(interactKey))
        {
            StartCoroutine(UseSwitch());
        }
    }

    private IEnumerator UseSwitch()
    {
        isInteracting = true;

        PlaySwitchOn();

        bool correct = false;

        if (puzzleManager != null)
        {
            correct = puzzleManager.TryCheckPuzzle();
        }
        else
        {
            Debug.LogWarning("ColorRunePuzzleManager is not assigned.");
        }

        if (correct)
        {
            isSolved = true;

            while (puzzleManager != null && puzzleManager.IsBusy())
            {
                yield return null;
            }

            PlaySwitchOn();

            isInteracting = false;
            yield break;
        }

        yield return new WaitForSeconds(switchOffDelay);

        PlaySwitchOff();

        while (puzzleManager != null && puzzleManager.IsBusy())
        {
            yield return null;
        }

        isInteracting = false;
    }

    private void PlaySwitchOn()
    {
        if (switchAnimator == null) return;

        switchAnimator.Play(switchOnStateName, 0, 0f);
    }

    private void PlaySwitchOff()
    {
        if (switchAnimator == null) return;

        switchAnimator.Play(switchOffStateName, 0, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}