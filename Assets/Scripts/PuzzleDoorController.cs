using UnityEngine;

public class PuzzleDoorController : MonoBehaviour
{
    public Animator doorAnimator;

    private bool isClosed = false;
    private bool isOpen = true;

    private void Awake()
    {
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }
    }

    public void CloseDoor()
    {
        if (isClosed) return;

        isClosed = true;
        isOpen = false;

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Close");
        }
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;
        isClosed = false;

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
    }
}