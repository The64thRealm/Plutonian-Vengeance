using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CollisionDamager : TagsBasedDamager
{
    protected override void Awake()
    {
        base.Awake();
        damageType = IDamager.DamageType.Collision;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = CheckAndGetDamageable(collision.gameObject);
        if (damageable != null)
        {
            DamageEntity(damageable);
        }
    }
}
