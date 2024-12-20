using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

/*
 * an AbstractAttackGroup is a type of attack that contains a bunch of other attacks
 * all children of this object must have an Attack component
 * this object sets all children to disabled upon being awoken
 */
public abstract class AbstractAttackGroup : AbstractAttack
{
    protected IAttack[] attacks;

    protected virtual void Awake()
    {
        List<IAttack> attackList = new();
        
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            IAttack attackComponent = child.GetComponent<IAttack>();
            if (attackComponent != null)
            {
                attackList.Add(child.GetComponent<IAttack>());
                child.SetActive(false);
            }
        }
        attacks = attackList.ToArray();
    }

    public override void Interrupt()
    {
        foreach (IAttack attack in attacks)
        {
            attack.Interrupt();
        }
        base.Interrupt();
    }

    /*
     * You can override the interrupt method using this method if for some
     * reason the group doesn't like the other interrupt method
     */
    protected void DefaultInterrupt()
    {
        base.Interrupt();
    }

    public override void Cleanup()
    {
        foreach (IAttack attack in attacks)
        {
            attack.Interrupt();
        }
    }
}
