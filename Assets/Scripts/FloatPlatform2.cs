using UnityEngine;

public class FloatPlatform2 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    [Header("Direction")]
    public bool moveVertical = true;

    [Header("Start State")]
    public bool activeOnStart = false;

    private Vector3 startPosition;
    private bool isActive = false;

    private void Start()
    {
        startPosition = transform.position;
        isActive = activeOnStart;
    }

    private void Update()
    {
        if (!isActive) return;

        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        if (moveVertical)
        {
            transform.position = startPosition + new Vector3(0f, offset, 0f);
        }
        else
        {
            transform.position = startPosition + new Vector3(offset, 0f, 0f);
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
        transform.position = startPosition;
    }
}