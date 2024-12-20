using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerDamager : TagsBasedDamager
{
    [Header("TriggerDamager")]
    [Tooltip("The amount of time to wait before doing another instance of damage if an object is inside of this trigger for long enough")]
    public float damageCooldown = 100;
    private Dictionary<IDamageable, float> currentlyDamagingObjects;

    protected override void Awake()
    {
        base.Awake();
        currentlyDamagingObjects = new();
        damageType = IDamager.DamageType.Beam;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        IDamageable damageable = CheckAndGetDamageable(collision.gameObject);
        if (damageable != null)
        {
            currentlyDamagingObjects.Add(damageable, damageCooldown);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable damageable = CheckAndGetDamageable(collision.gameObject);
        if (damageable != null)
        {
            currentlyDamagingObjects.Remove(damageable);
        }
    }

    private void Update()
    {
        // this seems really slow. It's making a new dictionary every loop to get rid of the ones that are null
        Dictionary<IDamageable, float> newMap = new();
        foreach (KeyValuePair<IDamageable, float> item in currentlyDamagingObjects.ToList())
        {
            if (item.Key == null || item.Key.Equals(null))
            {
                continue;
            }
            float newCooldown = item.Value - Time.deltaTime;
            if (newCooldown <= 0)
            {
                DamageEntity(item.Key);
                newCooldown = damageCooldown;
            }
            newMap[item.Key] = newCooldown;
        }
        currentlyDamagingObjects = newMap;
    }
}
