using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * runs all child attacks simultaneously until the first attack finishes
 */
public class SimultaneousDominantAttackGroup : AbstractAttackGroup
{
    public override void RestartAttack()
    {
        foreach (IAttack attack in attacks)
        {
            attack.GetGameObject().SetActive(true);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        attacks[0].GetDoneEvent().AddListener(StopAll);
        for (int i = 1; i < attacks.Length; ++i)
        {
            if (attacks[i].GetGameObject().GetComponent<LoopingContainer>() == null)
            {
                attacks[i].GetDoneEvent().AddListener(StopAttackUponFinish);
            }
        }
    }

    private void StopAll(IAttack.AttackContext context)
    {
        OnFinished?.Invoke(new IAttack.AttackContext(1, this));
        attacks[0].GetGameObject().SetActive(false);
        for (int i = 1; i < attacks.Length; ++i)
        {
            attacks[i].Interrupt();
        }
    }

    public void StopAttackUponFinish(IAttack.AttackContext context)
    {
        context.callingAttack.GetGameObject().SetActive(false);
    }
}
