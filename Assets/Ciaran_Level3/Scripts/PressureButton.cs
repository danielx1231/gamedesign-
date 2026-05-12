using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public Animator buttonAnim;
    public Animator platformAnim;

    public string buttonParam = "isPressed";
    public string platformParam = "isOpen";

    public bool animateButton = false;
    public string triggerLayerName = "Tigger";

    [Header("Gate Settings")]
    public Transform gateTarget;
    public Collider2D gateCollider;
    public string gateObjectName = "Gate";
    public Vector3 gateOpenOffset = new Vector3(0f, 3.1f, 0f);
    public float gateMoveDuration = 0.25f;
    public bool disableGateColliderWhenOpen = true;

    private readonly HashSet<Collider2D> pressingColliders = new HashSet<Collider2D>();
    private bool isPressed;
    private Vector3 gateClosedPosition;
    private Coroutine gateMoveRoutine;

    private void Awake()
    {
        if (animateButton && buttonAnim == null)
        {
            buttonAnim = GetComponent<Animator>();
        }

        if (!animateButton)
        {
            buttonAnim = null;
        }

        Rigidbody2D buttonBody = GetComponent<Rigidbody2D>();
        if (buttonBody != null)
        {
            buttonBody.bodyType = RigidbodyType2D.Static;
            buttonBody.gravityScale = 0f;
        }

        ResolveGateReferences();
        SetState(false, true);
    }

    private void Start()
    {
        ResolveGateReferences();
    }

    private void ResolveGateReferences()
    {
        if (gateTarget == null && !string.IsNullOrEmpty(gateObjectName))
        {
            GameObject gateObject = GameObject.Find(gateObjectName);
            if (gateObject != null)
            {
                gateTarget = gateObject.transform;
            }
        }

        if (gateCollider == null && gateTarget != null)
        {
            gateCollider = gateTarget.GetComponent<Collider2D>();
        }

        if (gateTarget != null)
        {
            gateClosedPosition = gateTarget.position;
        }
    }

    private bool IsValid(Collider2D other)
    {
        return other.gameObject.layer == LayerMask.NameToLayer(triggerLayerName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsValid(other)) return;

        pressingColliders.Add(other);
        SetState(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsValid(other)) return;

        pressingColliders.Remove(other);
        if (pressingColliders.Count == 0) SetState(false);
    }

    private void SetState(bool pressed, bool instant = false)
    {
        if (isPressed == pressed && !instant) return;

        isPressed = pressed;
        if (animateButton && buttonAnim) buttonAnim.SetBool(buttonParam, pressed);
        if (platformAnim) platformAnim.SetBool(platformParam, pressed);

        MoveGate(pressed, instant);
    }

    private void MoveGate(bool open, bool instant)
    {
        if (gateTarget == null) return;

        Vector3 targetPosition = gateClosedPosition + (open ? gateOpenOffset : Vector3.zero);

        if (gateMoveRoutine != null)
        {
            StopCoroutine(gateMoveRoutine);
            gateMoveRoutine = null;
        }

        if (disableGateColliderWhenOpen && gateCollider != null)
        {
            gateCollider.enabled = !open;
        }

        if (instant || gateMoveDuration <= 0f)
        {
            gateTarget.position = targetPosition;
            return;
        }

        gateMoveRoutine = StartCoroutine(MoveGateRoutine(targetPosition));
    }

    private IEnumerator MoveGateRoutine(Vector3 targetPosition)
    {
        Vector3 startPosition = gateTarget.position;
        float elapsed = 0f;

        while (elapsed < gateMoveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / gateMoveDuration);
            gateTarget.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        gateTarget.position = targetPosition;
        gateMoveRoutine = null;
    }
}
