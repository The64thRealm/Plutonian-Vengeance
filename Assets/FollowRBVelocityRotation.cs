using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRBVelocityRotation : MonoBehaviour
{
    public Rigidbody2D rb;
    public float angleOffset;

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity) * Quaternion.AngleAxis(angleOffset, Vector3.forward);
    }
}
