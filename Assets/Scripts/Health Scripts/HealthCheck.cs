using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Processors;

public class HealthCheck : MonoBehaviour, IDamageable
{
    public UnityEvent<IDamageable.DamagedInfo> OnDamaged = new();
    public UnityEvent<IDamageable.DamagedInfo> OnDeath = new();

    public float maxHealth = 100;
    public float currentHealth;
    [Tooltip("The amount of time that something will be \"invincible\" after it takes damage. During this \"invincibility\" period, you will only take the the maximum damage on any damagers that you touched")]
    public float invincibilityTime = 0;

    // how long until you're  not invincible anymore. Updated in updates
    private float invincibilityCooldown;
    // the largest amount of damage taken within this invincibility cooldown. Used to check if you need to take more damage
    private float highestDamage;
    [SerializeField] private bool destroyOnDeath = false;

    public bool immune { get; set; }

    public bool isDead { get; private set; }

    public HealthBar healthBar;

    private void Awake()
    {
        isDead = false;
        immune = false;
        invincibilityCooldown = 0;
        highestDamage = 0;
        currentHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth((int)maxHealth);
    }

    public void TakeDamage(IDamager.Damage damageInfo)
    {
        if (immune) return;
        //Debug.Log($"{damageInfo.damageAmount} from {damageInfo.damager.name}");
        // checking if you take damage or not is handled in the Damager class

        // if you don't take damage, why tf was this called
        if (damageInfo.damageAmount <= 0)
        {
            return;
        }

        // handles the invincibility cooldown
        float damageToTake = damageInfo.damageAmount;
        if (damageInfo.damageType != IDamager.DamageType.Beam)
        {
            if (invincibilityCooldown <= 0)
            {
                // if we aren't invincible, then reset the invincibility time and set this to the max damage that was taken
                invincibilityCooldown = invincibilityTime;
                highestDamage = damageToTake;
            }
            else if (damageToTake > highestDamage)
            {
                damageToTake -= highestDamage;
                highestDamage = damageInfo.damageAmount;
            }
            else
            {
                damageToTake = 0;
            }
        }
        currentHealth -= damageToTake;
        IDamageable.DamagedInfo info = new(currentHealth, damageToTake, damageInfo);
        OnDamaged?.Invoke(info);
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke(info);
            healthBar.SetHealth(0);
            isDead = true;
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            healthBar.SetHealth((int)currentHealth);
        }
    }

    private void Update()
    {
        invincibilityCooldown -= Time.deltaTime;
    }
}
