using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * This class does not do tracking
 */
public class DashAt : AbstractMoveAttack {
    [Header("DashAt")]
    [Tooltip("The amount to accelerate directly toward the waypoint. The object can have extra perpendicular acceleration, though, so the actual acceleration is likely higher")]
    [SerializeField] private float acceleration;
    [Tooltip("The maximum allowed speed that the object can accelerate to directly toward the waypoint. This does not account for perpendicular speed. This also does not limit the speed if the speed was already higher")]
    [SerializeField] private float maxSpeed;
    [Tooltip("how long you allow the dash to go for")]
    [SerializeField] private float dashTime;
    [Tooltip("target object, if you want to target an object.")]
    [SerializeField] private GameObject target;
    [Tooltip("target position, if you want to target a set position. This is ignored if the target object is set")]
    [SerializeField] private Vector2 targetPos;

    float timeActive;
    Vector2 initialRelative;
    bool targetReached;

    private void FixedUpdate()
    {
        timeActive += Time.fixedDeltaTime;
        if (timeActive > dashTime)
        {
            OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
        }
        if (targetReached)
        {
            return;
        }
        // if we're on the other side of the target then we've reached our destination
        Vector2 relative = targetPos - attackerBody.position;
        if (Vector2.Dot(initialRelative, relative) < 0)
        {
            targetReached = true;
        }
        Vector2 forceDirection = Vector2.zero;
        Vector2 velocityProjection = Vector3.Project(attackerBody.velocity, relative);
        // if our velocity is more than 0 we should correct our velocity to go toward the object
        if (attackerBody.velocity.sqrMagnitude > 0)
        {
            // take into account the current velocity of the body
            Vector2 orthogonalVel = attackerBody.velocity - velocityProjection;
            forceDirection -= orthogonalVel;
        }
        // if we are moving away from the object or we are below our max speed, add in the relative portion of the force
        if (Vector2.Dot(velocityProjection, relative) <= 0 || velocityProjection.sqrMagnitude <= maxSpeed * maxSpeed)
        {
            forceDirection += relative;
        }
        // normalize to acceleration
        forceDirection = forceDirection.normalized * acceleration;
        attackerBody.AddForce(forceDirection);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target == null ? targetPos : target.transform.position, 0.2f);
    }

    public override void RestartAttack()
    {
        targetReached = false;
        timeActive = 0;
        if (target == null)
        {
            if (targetPos == null)
            {
                throw new NullReferenceException($"No target was given to the {name} dash attack");
            }
        }
        else
        {
            targetPos = target.transform.position;
        }
        initialRelative = targetPos - attackerBody.position;
    }
}
