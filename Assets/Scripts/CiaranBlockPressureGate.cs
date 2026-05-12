using UnityEngine;

public class CiaranBlockPressureGate : MonoBehaviour
{
    [Header("Required Object")]
    public Transform requiredBlock;

    [Header("Gate")]
    public Transform gate;
    public Vector3 gateOpenOffset = new Vector3(0, 3.1f, 0);
    public float gateMoveSpeed = 3f;

    [Header("Behaviour")]
    public bool stayOpenOncePressed = true;

    private Vector3 gateClosedPosition;
    private Vector3 gateOpenPosition;
    private bool isPressed = false;
    private bool hasOpened = false;

    private void Start()
    {
        if (gate != null)
        {
            gateClosedPosition = gate.position;
            gateOpenPosition = gateClosedPosition + gateOpenOffset;
        }
    }

    private void Update()
    {
        if (gate == null) return;

        bool shouldOpen = stayOpenOncePressed ? hasOpened : isPressed;

        Vector3 targetPosition = shouldOpen ? gateOpenPosition : gateClosedPosition;

        gate.position = Vector3.MoveTowards(
            gate.position,
            targetPosition,
            gateMoveSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (requiredBlock == null) return;

        if (other.transform == requiredBlock || other.transform.IsChildOf(requiredBlock))
        {
            isPressed = true;
            hasOpened = true;
            Debug.Log("Block pressed the button. Gate opening.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (requiredBlock == null) return;

        if (other.transform == requiredBlock || other.transform.IsChildOf(requiredBlock))
        {
            isPressed = false;
            Debug.Log("Block left the button.");
        }
    }
}