using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TweenIn : MonoBehaviour
{
    public Vector2 targetPosition;

    public float percentage;

    public UnityEvent ReactionEvent = new();

    public float tolerance = 100f;

    private void FixedUpdate()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, percentage);
        newPos.z = transform.position.z;

        transform.position = newPos;

        if (((Vector2) transform.position - targetPosition).sqrMagnitude <= tolerance*tolerance)
        {
            ReactionEvent?.Invoke();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetPosition, tolerance);
    }
}
