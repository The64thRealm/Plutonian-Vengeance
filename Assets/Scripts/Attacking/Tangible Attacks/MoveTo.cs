using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/*
 * This class does not do tracking
 */
public class MoveTo : AbstractMoveAttack
{
    [Header("MoveTo")]
    [Tooltip("This event is called when the enemy reaches a waypoint with the waypoint index passed in as an argument")]
    public UnityEvent<int> wayPointReachedEvent;
    [Tooltip("The amount to accelerate directly toward the waypoint. The object can have extra perpendicular acceleration, though, so the actual acceleration is likely higher")]
    [SerializeField] float acceleration;
    [Tooltip("The maximum allowed speed that the object can accelerate to directly toward the waypoint. This does not account for perpendicular speed. This also does not limit the speed if the speed was already higher")]
    [SerializeField] float maxSpeed;
    [Tooltip("Set of waypoints to visit")]
    [SerializeField] Waypoint[] waypoints;
    int currWaypoint;
    Vector2 initialRelative;
    Vector2 relative;

    /*
     * DO NOT pass in a negative number to this method
     */
    private Vector2 getTargetPos(int waypointToGet = -1)
    {
        if (waypointToGet <= -1)
        {
            return waypoints[currWaypoint].GetPosition();
        }
        return waypoints[waypointToGet].GetPosition();
    }

    private void FixedUpdate()
    {
        if (Vector2.Dot(initialRelative, relative) < 0)
        {
            wayPointReachedEvent?.Invoke(currWaypoint);
            ++currWaypoint;
            if (currWaypoint >= waypoints.Length)
            {
                OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
            } else
            {
                initialRelative = getTargetPos() - attackerBody.position;
            }
        }
        relative = getTargetPos() - attackerBody.position;
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
        if (waypoints == null) return;
        Vector2 prevWaypoint = Vector2.zero;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; ++i)
        {
            if (i > 0)
            {
                float arrowHeadAngle = 15;
                float arrowHeadLength = 0.1f;
                Vector2 direction = getTargetPos(i) - prevWaypoint;
                Gizmos.DrawRay(prevWaypoint, direction);
                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle + 180, 0, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle + 180, 0, 0) * new Vector3(0, 0, 1);
                Gizmos.DrawRay(getTargetPos(i), right * arrowHeadLength);
                Gizmos.DrawRay(getTargetPos(i), left * arrowHeadLength);
            }
            Gizmos.DrawWireSphere(getTargetPos(i), 0.2f);
            prevWaypoint = getTargetPos(i);
        }
    }

    public override void RestartAttack()
    {
        currWaypoint = 0;
        initialRelative = getTargetPos() - attackerBody.position;
        relative = getTargetPos() - attackerBody.position;
    }
}

[System.Serializable]
public struct Waypoint
{
    [Header("Waypoint")]
    /*
     * only set the one you wish to follow, if target is set, then
     * position will be ignored
     */
    [Tooltip("Object to target with the waypoint. Do not set this if you wish to target a position instead")]
    public GameObject gameObjectTarget;
    [Tooltip("Position to target with this waypoint. Do not set the gameObjectTarget if you wish to use this")]
    public Vector2 position;
    public Vector2 GetPosition()
    {
        if (gameObjectTarget != null)
        {
            return gameObjectTarget.transform.position;
        }
        if (position == null)
        {
            throw new NullReferenceException($"No target was given to one of the waypoints in a MoveTo attack");
        }
        return position;
    }
}