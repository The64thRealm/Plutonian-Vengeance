using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyOnHit : MonoBehaviour, IDamageable
{
    public UnityEvent OnDestroy = new();
    public void TakeDamage(IDamager.Damage damageInfo)
    {
        OnDestroy?.Invoke();
        Destroy(gameObject);
    }
}
