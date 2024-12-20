using System;
using UnityEngine;
using UnityEngine.Events;

/*
 * This class does not do tracking
 */
public class RotateAmount : AbstractMoveAttack
{
    [Header("RotateAmount")]
    [Tooltip("The amount to rotationally accelerate in the direction")]
    [SerializeField] float torque;
    [Tooltip("The maximum allowed rotational speed")]
    [SerializeField] float maxRotationalSpeed;
    [Tooltip("The total amount to rotate")]
    [SerializeField] float rotateAmount;
    private float initialRotation;
    private float targetRotation;

    private void FixedUpdate()
    {
        if (Mathf.Sign(attackerBody.angularVelocity) != Mathf.Sign(rotateAmount) || Math.Abs(attackerBody.angularVelocity) < maxRotationalSpeed)
        {
            attackerBody.AddTorque(torque * Mathf.Sign(targetRotation - attackerBody.rotation));
        }
        if (Mathf.Sign(targetRotation - attackerBody.rotation) != Mathf.Sign(targetRotation - initialRotation))
        {
            OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
        }
    }

    public override void RestartAttack()
    {
        initialRotation = attackerBody.rotation;
        targetRotation = attackerBody.rotation + rotateAmount;
    }
}