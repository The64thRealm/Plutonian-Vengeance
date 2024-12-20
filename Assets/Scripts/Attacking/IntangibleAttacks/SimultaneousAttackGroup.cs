using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimultaneousAttackGroup : AbstractAttackGroup
{
    [Header("SimultaneousAttackGroup")]
    [Tooltip("Set this to true for this to run indefinitely. By default, the simultaneous attack group stops when all children have finished once, but this bool disables that behavior. Beware that turning this setting to true will make all parent attacks also run indefinitely")]
    [SerializeField] private bool runIndefinitely = false;
    private int numChildrenDone;

    protected override void Awake()
    {
        base.Awake();
        if (!runIndefinitely)
        {            foreach (IAttack attack in attacks)
            {
                attack.GetDoneEvent().AddListener(ChildDone);
            }
        }
    }

    public override void RestartAttack()
    {
        numChildrenDone = 0;
        foreach (IAttack attack in attacks)
        {
            if (attack.GetGameObject().activeInHierarchy)
            {
                attack.RestartAttack();
                attack.ResetFinishCount();
            } else
            {
                attack.GetGameObject().SetActive(true);
            }
        }
    }

    public void ChildDone(IAttack.AttackContext context)
    {
        if (context.finishedCount == 1)
        {
            ++numChildrenDone;
            if (context.callingAttack.GetGameObject().GetComponent<LoopingContainer>() == null) // check for looping type
            {
                context.callingAttack.GetGameObject().SetActive(false);
            }
            if (numChildrenDone == attacks.Length)
            {
                OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
            }
        }
    }
}
