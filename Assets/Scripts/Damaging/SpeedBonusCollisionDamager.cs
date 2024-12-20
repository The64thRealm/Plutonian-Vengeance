using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SpeedBonusCollisionDamager : SpeedScalingCollisionDamager
{
    [Header("SpeedBonusCollisionDamager")]
    [Tooltip("The speed at which to apply the damage bonus")]
    [SerializeField] private float bonusSpeed = 20;
    [Tooltip("The damage bonus to apply")]
    [SerializeField] private float damageBonus = 20;

    [SerializeField] private GameObject numberParticle;

    [Tooltip("This event will be invoked when the asteroid does bonusDamage")]
    public UnityEvent OnDealtBonusDamage = new();
    [Tooltip("This event will be invoked when the asteroid reaches the bonusSpeed")]
    public UnityEvent OnSurpassedSpeed = new();
    [Tooltip("This event will be invoked when the asteroid was past the bonusSpeed, but slowed down and is now slower")]
    public UnityEvent OnSlowedDown = new();

    private bool surpassedSpeed;
    private Rigidbody2D rigidBody2d;

    protected override void Awake()
    {
        base.Awake();
        surpassedSpeed = false;
        rigidBody2d = GetComponent<Rigidbody2D>();

        OnDealtDamage.AddListener(SpawnPointNumber);
    }

    public void SpawnPointNumber(IDamager.Damage damage)
    {
        // TODO this should probably not be in here
        int damageNumber = Mathf.RoundToInt(damage.damageAmount * 10) * 10;

        GameObject damagePointIndicator = Instantiate(numberParticle, transform.position, Quaternion.identity);
        GameObject numberObject = damagePointIndicator.transform.GetChild(0).gameObject;
        numberObject.GetComponent<TextMeshProUGUI>().text = damageNumber.ToString();
        Destroy(damagePointIndicator, 2);
    }

    private void FixedUpdate()
    {
        if (surpassedSpeed && rigidBody2d.velocity.sqrMagnitude < bonusSpeed * bonusSpeed)
        {
            surpassedSpeed = false;
            OnSlowedDown?.Invoke();
            //Debug.Log("Speed Reached");
        } else if (!surpassedSpeed && rigidBody2d.velocity.sqrMagnitude >= bonusSpeed * bonusSpeed)
        {
            surpassedSpeed = true;
            OnSurpassedSpeed?.Invoke();
            //Debug.Log("Speed Lost");
        }
    }

    public override float CalculateDamage()
    {
        return base.CalculateDamage() + (surpassedSpeed ? damageBonus : 0);
    }

    public override void DamageEntity(IDamageable entity)
    {
        base.DamageEntity(entity);
        if (surpassedSpeed)
        {
            OnDealtBonusDamage?.Invoke();
        }
    }
}
