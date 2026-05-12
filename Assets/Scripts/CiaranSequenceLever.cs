using UnityEngine;

public class CiaranSequenceLever : MonoBehaviour
{
    [Header("Lever Settings")]
    public int leverNumber = 1;
    public bool isResetLever = false;

    [Header("Player Detection")]
    public string playerTag = "Player";
    private bool playerInRange = false;

    [Header("Lever State")]
    public bool isPulled = false;

    [Header("Visual Rotation")]
    public bool rotateLeverVisual = true;
    public float pulledRotationZ = -45f;

    [Header("Puzzle Manager")]
    public CiaranSequencePuzzleManager puzzleManager;

    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (isResetLever)
        {
            Debug.Log("Reset lever pulled.");

            if (puzzleManager != null)
            {
                puzzleManager.ResetPuzzle();
            }

            PlayResetVisual();
            return;
        }

        isPulled = !isPulled;
        UpdateLeverVisual();

        Debug.Log("Lever " + leverNumber + " interacted.");

        if (puzzleManager != null)
        {
            puzzleManager.RegisterLeverInput(leverNumber);
        }
    }

    public void ResetLeverState()
    {
        isPulled = false;
        UpdateLeverVisual();
    }

    private void UpdateLeverVisual()
    {
        if (!rotateLeverVisual) return;

        if (isPulled)
        {
            transform.rotation = Quaternion.Euler(
                originalRotation.eulerAngles.x,
                originalRotation.eulerAngles.y,
                pulledRotationZ
            );
        }
        else
        {
            transform.rotation = originalRotation;
        }
    }

    private void PlayResetVisual()
    {
        if (!rotateLeverVisual) return;

        transform.rotation = Quaternion.Euler(
            originalRotation.eulerAngles.x,
            originalRotation.eulerAngles.y,
            pulledRotationZ
        );

        Invoke(nameof(ResetLeverVisualOnly), 0.3f);
    }

    private void ResetLeverVisualOnly()
    {
        transform.rotation = originalRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            Debug.Log("Player near lever " + leverNumber + ". Press E.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
        }
    }
}