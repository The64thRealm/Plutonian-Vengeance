using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class does not inherit from moveAttack anymore because it doesn't
 * rely on a rigid body nor will it ever need to use a shooting event
 */
public class Teleport : AbstractAttack
{
    [Header("Teleport")]
    [Tooltip("The attacker to move")]
    public GameObject attackerObject;
    [Tooltip("set this to true if you want the object to reset its velocity upon teleporting, and false if you want it to keep its velocity. This will only work if the game object you gave has a rigid body")]
    [SerializeField] bool resetVelocity = true;
    [Tooltip("target object, if you want to target an object")]
    [SerializeField] GameObject target;
    [Tooltip("target position, if you want to target a set position")]
    [SerializeField] Vector2 targetPos;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target == null ? targetPos : target.transform.position, 0.2f);
    }

    public override void RestartAttack()
    {
        if (target == null)
        {
            if (targetPos == null)
            {
                throw new NullReferenceException($"No target was given to the {name} teleport attack");
            }
        }
        else
        {
            targetPos = target.transform.position;
        }

        Rigidbody2D attackerBody = attackerObject.GetComponent<Rigidbody2D>();

        if (attackerBody != null)
        {
            attackerBody.position = targetPos;
            if (resetVelocity)
            {
                attackerBody.velocity = Vector2.zero;
            }
        } else
        {
            attackerObject.transform.position = targetPos;
        }

        OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
    }
}
