using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoTelemetry : MonoBehaviour
{
    private HealthCheck plutoHealth;
    private void Awake()
    {
        plutoHealth = GetComponent<HealthCheck>();
        plutoHealth.OnDamaged.AddListener(LogPlutoDamaged);
        plutoHealth.OnDeath.AddListener(LogPlutoDeath);
    }

    private void OnDestroy()
    {
        plutoHealth.OnDamaged.RemoveListener(LogPlutoDamaged);
        plutoHealth.OnDeath.RemoveListener(LogPlutoDeath);
    }

    public void LogPlutoDamaged(IDamageable.DamagedInfo info)
    {
        GameManager.instance.PlayerDamaged(info.attackStr, info.attemptedDamage);
    }

    public void LogPlutoDeath(IDamageable.DamagedInfo info)
    {
        GameManager.instance.PlayerDied(info.attackStr, info.attemptedDamage);
    }
}
