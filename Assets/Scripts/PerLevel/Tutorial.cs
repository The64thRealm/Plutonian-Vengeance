using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public HealthCheck centerAsteroid;
    public GameObject healthBar;

    public AttackManager attackManager;
    public BroadcastEvent EnemyDeathEvent;
    public BroadcastEvent capturedEvent;
    public BroadcastEvent surpassedSpeedEvent;

    private void Start()
    {
        centerAsteroid.currentHealth = 100000;
    }

    public void DialogueAdvancedTo(int line)
    {
        if (line == 5)
        {
            attackManager.enabled = true;
        } else if (line == 8)
        {
            centerAsteroid.currentHealth = 40;
            healthBar.SetActive(true);
        }
    }

    public void ActivateCapturedEvent()
    {
        capturedEvent.Trigger();
    }

    public void ActivateEnemyDeathEvent()
    {
        EnemyDeathEvent.Trigger();
    }
    public void ActivateSurpassedSpeedEvent()
    {
        surpassedSpeedEvent.Trigger();
    }
}
