using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCheckTest : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<AntiPlayerBool>().asteroid == true)
        {
            TakeDamage(10);
        }

        if (collision.gameObject.GetComponent<AntiPlayerBool>().planet == true)
        {
            TakeDamage(40);
        }

        if (currentHealth == (0))
        {
            Destroy(gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
