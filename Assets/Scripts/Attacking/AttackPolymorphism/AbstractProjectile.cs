using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TagsBasedDamager))]
public abstract class AbstractProjectile : MonoBehaviour
{
    /*
     * whether the projectile looks in the direction it's moving in
    */
    [SerializeField] public bool faceMovementDirection;
    [SerializeField] public Vector2 movementVector;
    [SerializeField] private float approximateRadiusOfBody = 0.25f;
    public float approximateRadius => approximateRadiusOfBody;
    public const float KILLZONE_BOUNDS_OFFSET = 4;
    public TagsBasedDamager damager { get; private set; }
    protected Rigidbody2D projectileBody;

    protected virtual void Awake()
    {
        damager = GetComponent<TagsBasedDamager>();
        projectileBody = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {
        if (Util.CheckIfInCameraBounds(transform.position, KILLZONE_BOUNDS_OFFSET)) { Destroy(gameObject); }
    }
    protected virtual void OnDisable()
    {
        damager.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Util.DrawBounds(KILLZONE_BOUNDS_OFFSET);
    }
}
