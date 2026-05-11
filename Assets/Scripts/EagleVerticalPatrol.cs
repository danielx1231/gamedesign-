using UnityEngine;

public class EagleVerticalPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movement")]
    public float speed = 3f;

    private Transform targetPoint;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (pointA == null || pointB == null)
        {
            Debug.LogWarning("Point A or Point B is not assigned on " + gameObject.name);
            return;
        }

        targetPoint = pointB;

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (pointA == null || pointB == null) return;

        MoveBetweenPoints();
    }

    private void MoveBetweenPoints()
    {
        Vector2 currentPosition = rb != null ? rb.position : (Vector2)transform.position;
        Vector2 targetPosition = targetPoint.position;

        Vector2 newPosition = Vector2.MoveTowards(
            currentPosition,
            targetPosition,
            speed * Time.fixedDeltaTime
        );

        if (rb != null)
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            transform.position = newPosition;
        }

        if (Vector2.Distance(newPosition, targetPosition) < 0.05f)
        {
            if (targetPoint == pointA)
            {
                targetPoint = pointB;
            }
            else
            {
                targetPoint = pointA;
            }
        }
    }
}