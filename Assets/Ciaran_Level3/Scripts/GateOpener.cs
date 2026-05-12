using System;
using UnityEngine;

public class GateOpener : MonoBehaviour
{
    [Header("Lever Settings")]
    public Animator switchAnimator;
    public Animator gateAnimator;

    private bool isPlayerInRange;
    private bool isPulled;
    private LeverGroupActivator leverGroup;

    public bool IsPulled => isPulled;

    private void Awake()
    {
        if (switchAnimator == null)
        {
            switchAnimator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        leverGroup = FindObjectOfType<LeverGroupActivator>();
        if (leverGroup != null)
        {
            leverGroup.RegisterLever(this);
        }
    }

    private void Update()
    {
        if (!isPulled && isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerMechanic();
        }
    }

    private void TriggerMechanic()
    {
        isPulled = true;

        if (switchAnimator != null)
        {
            switchAnimator.SetTrigger("Open");
        }

        if (leverGroup != null)
        {
            leverGroup.NotifyLeverChanged();
        }
        else if (gateAnimator != null)
        {
            gateAnimator.SetTrigger("Open");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other))
        {
            isPlayerInRange = true;
            Debug.Log("Near lever, press E to interact");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayer(other))
        {
            isPlayerInRange = false;
        }
    }

    private bool IsPlayer(Collider2D other)
    {
        return other.CompareTag("Player") || other.gameObject.name.StartsWith("player", StringComparison.OrdinalIgnoreCase);
    }
}
