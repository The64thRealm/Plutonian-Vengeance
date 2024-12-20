using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TagsBasedDamager : MonoBehaviour, IDamager
{
    public TagsList tagsToDamage;

    public float damageToDo;
    public IDamager.DamageType damageType { get; set; }
    public string damageDescription { get; private set; }
    public UnityEvent<IDamager.Damage> OnDealtDamage = new();

    protected virtual void Awake()
    {
        damageDescription = name;
    }

    public virtual void DamageEntity(IDamageable entity)
    {
        IDamager.Damage damage = new IDamager.Damage(damageType, CalculateDamage(), gameObject, damageDescription);
        OnDealtDamage?.Invoke(damage);
        entity.TakeDamage(damage);
    }

    /*
     * Checks if the obj has one of the specified tags and has a damageable component, and returns it if it does
     * returns null if the obj doesn't have any of the tags or if it doesn't have a damageable component
     */
    protected IDamageable CheckAndGetDamageable(GameObject obj)
    {
        return CheckAndGetDamageable(obj, tagsToDamage);
    }

    protected IDamageable CheckAndGetDamageable(GameObject obj, TagsList tags)
    {
        foreach (string tag in tags.tags)
        {
            if (obj.CompareTag(tag))
            {
                return obj.GetComponent<IDamageable>();
            }
        }
        return null;
    }

    public void AddToDamageDescription(string description)
    {
        damageDescription = $"{description}:{damageDescription}";
    }

    /*
     * overload this method if you want to calculate the damage in some other way
     */
    public virtual float CalculateDamage()
    {
        return damageToDo;
    }

    public UnityEvent<IDamager.Damage> GetDealtDamageEvent()
    {
        return OnDealtDamage;
    }
}
