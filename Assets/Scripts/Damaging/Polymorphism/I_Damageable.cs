using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamager;

public interface IDamageable
{
    readonly struct DamagedInfo
    {
        public DamagedInfo(float resultantHealth, float actualDamage, IDamager.Damage damage) {
            damageType = damage.damageType;
            this.resultantHealth = resultantHealth;
            this.actualDamage = actualDamage;
            attemptedDamage = damage.damageAmount;
            damager = damage.damager;
            attackStr = damage.attackStr;
        }
        public DamageType damageType { get; }
        public float actualDamage { get; }
        public float attemptedDamage { get; }
        public GameObject damager { get; }
        public string attackStr { get; }
        public float resultantHealth { get; }
    }
    public void TakeDamage(IDamager.Damage damageInfo);
}
