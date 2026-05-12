using UnityEngine;

public class CiaranUnlockPushBlock : MonoBehaviour
{
    [Header("Player Detection")]
    public string playerTag = "Player";
    private bool playerInRange = false;
    private bool unlocked = false;

    [Header("Target Block")]
    public Rigidbody2D targetBlockRb;
    public Collider2D targetBlockCollider;

    [Header("Switch Animation")]
    public Animator switchAnimator;
    public string triggerName = "Open";

    [Header("Block Physics After Unlock")]
    public float blockMass = 5f;
    public float blockLinearDrag = 2f;
    public float blockGravityScale = 1f;

    void Update()
    {
        if (playerInRange && !unlocked && Input.GetKeyDown(KeyCode.E))
        {
            UnlockBlock();
        }
    }

    private void UnlockBlock()
    {
        unlocked = true;
        Debug.Log("Block unlocked and can now be pushed.");

        if (switchAnimator != null)
        {
            switchAnimator.SetTrigger(triggerName);
        }

        if (targetBlockRb != null)
        {
            targetBlockRb.bodyType = RigidbodyType2D.Dynamic;
            targetBlockRb.mass = blockMass;
            targetBlockRb.drag = blockLinearDrag;
            targetBlockRb.gravityScale = blockGravityScale;
            targetBlockRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (targetBlockCollider != null)
        {
            targetBlockCollider.enabled = true;
            targetBlockCollider.isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            Debug.Log("Player near lever. Press E to unlock block.");
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