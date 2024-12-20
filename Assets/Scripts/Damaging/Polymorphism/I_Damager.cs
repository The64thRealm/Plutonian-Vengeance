using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamager
{
    public enum DamageType { Collision, Beam };

    public readonly struct Damage
    {
        public Damage(DamageType damageType, float damageAmount, GameObject damager, string attackStr)
        {
            this.damageType = damageType;
            this.damageAmount = damageAmount;
            this.damager = damager;
            this.attackStr = attackStr;
        }
        public DamageType damageType { get; }
        public float damageAmount { get; }
        public GameObject damager { get; }
        public string attackStr { get; }
    }
    public UnityEvent<Damage> GetDealtDamageEvent();

    public void DamageEntity(IDamageable entity); // must pass Damage struct to the entity
}
