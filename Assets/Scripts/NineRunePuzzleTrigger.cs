using UnityEngine;
using System.Collections;

public class NineRunePuzzleTrigger : MonoBehaviour
{
    [Header("Puzzle Manager")]
    public NineRunePuzzleManager puzzleManager;

    [Header("Switch Animation")]
    public Animator switchAnimator;
    public string openBoolName = "Open";

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

        SetSwitchOff();
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

        // 先播放机关打开动画
        SetSwitchOn();

        bool correct = false;

        if (puzzleManager != null)
        {
            correct = puzzleManager.TryCheckPuzzle();
        }
        else
        {
            Debug.LogWarning("NineRunePuzzleManager is not assigned.");
        }

        // 正确：机关保持 on，不能再交互
        if (correct)
        {
            isSolved = true;

            while (puzzleManager != null && puzzleManager.IsBusy())
            {
                yield return null;
            }

            SetSwitchOn();

            isInteracting = false;
            yield break;
        }

        // 错误：等一下，然后机关回到 off
        yield return new WaitForSeconds(switchOffDelay);

        SetSwitchOff();

        while (puzzleManager != null && puzzleManager.IsBusy())
        {
            yield return null;
        }

        isInteracting = false;
    }

    private void SetSwitchOn()
    {
        if (switchAnimator == null) return;

        switchAnimator.SetBool(openBoolName, true);
    }

    private void SetSwitchOff()
    {
        if (switchAnimator == null) return;

        switchAnimator.SetBool(openBoolName, false);
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