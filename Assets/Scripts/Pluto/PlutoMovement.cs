using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoMovement : MonoBehaviour
{
    [SerializeField] private float acceleration = 20f;
    // a mouthful but this is the maximum that the velocity will be before we stop accelerating it in that direction
    [SerializeField] private float maxVelocity = 7f;
    private Pluto pluto;
    private Rigidbody2D rigidBody2d;

    private void Awake()
    {
        pluto = GetComponent<Pluto>();
        rigidBody2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 force = pluto.plutoInputHandler.playerInputActions.Pluto.Move.ReadValue<Vector2>().normalized * acceleration;

        float dotProduct = Vector2.Dot(force, rigidBody2d.velocity.normalized);
        if ((rigidBody2d.velocity.sqrMagnitude > maxVelocity * maxVelocity) && dotProduct >= 0)
        {
            force = force - dotProduct * rigidBody2d.velocity.normalized;
        }
        rigidBody2d.AddForce(force, ForceMode2D.Force);
    }
}
