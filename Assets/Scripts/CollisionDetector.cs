using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField]
    private string _targetTag = "Player"; // We'll look for this tag instead of a script

    [SerializeField]
    private UnityEvent _collisionEntered;

    [SerializeField]
    private UnityEvent _collisionExit;

    private void OnCollisionEnter2D(Collision2D col)
    {
        // CompareTag is faster and cleaner than GetComponent
        if (col.gameObject.CompareTag(_targetTag))
        {
            _collisionEntered?.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(_targetTag))
        {
            _collisionExit?.Invoke();
        }
    }
}