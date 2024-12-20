using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoopingContainer : AbstractAttack
{
    [Header("LoopingContainer")]
    [Tooltip("setting this value to true will mean that the attack will continue looping until this object is explicitly told to disable or be interrupted. False will make this attack expect to be stopped as soon as it loops timesToLoop number of times")]
    [SerializeField] private bool loopIndefinitely = false;
    [Tooltip("Loops this many times before it finishes. If loopIndefinitely is set to true, will continue looping after finishing and invoking the OnFinished event for every timesToLoop loops. Must be 1 or more")]
    [SerializeField, Range(1, 100)] private int timesToLoop = 1;
    private IAttack loopedAttack;

    private void Awake()
    {
        // find the attack to loop
        foreach (Transform child in transform)
        {
            IAttack childAtk = child.gameObject.GetComponent<IAttack>();
            if (childAtk != null)
            {
                loopedAttack = childAtk;
                break;
            }
        }
        if (loopedAttack == null)
        {
            throw new System.ArgumentException($"No attack type under {gameObject.name} looped attack");
        }

        loopedAttack.GetDoneEvent().AddListener(AttackDone);
    }

    public override void RestartAttack()
    {
        loopedAttack.GetGameObject().SetActive(false);
        loopedAttack.GetGameObject().SetActive(true);
    }

    /*
     * loop the attack if appropriate, otherwise, don't
     */
    protected void AttackDone(IAttack.AttackContext context)
    {
        if (loopIndefinitely)
        {
            OnFinished?.Invoke(new IAttack.AttackContext(context.finishedCount, this));
            context.callingAttack.RestartAttack();
        }
        else if (context.finishedCount >= timesToLoop)
        {
            loopedAttack.GetGameObject().SetActive(false);
            OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
        }
        else
        {
            context.callingAttack.RestartAttack();
        }
    }

    public override void Interrupt()
    {
        base.Interrupt();
        loopedAttack.Interrupt();
    }
}
