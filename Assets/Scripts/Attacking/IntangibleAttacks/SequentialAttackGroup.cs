using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;


public class SequentialAttackGroup : AbstractAttackGroup
{
    private int _currAtkIndex;

    protected override void Awake()
    {
        base.Awake();
        foreach (IAttack attack in attacks)
        {
            attack.GetDoneEvent().AddListener(OnChildAttackFinished);
        }
    }

    public override void RestartAttack()
    {
        _currAtkIndex = 0;
        attacks[_currAtkIndex].GetGameObject().SetActive(true);
    }

    public void OnChildAttackFinished(IAttack.AttackContext context)
    {
        attacks[_currAtkIndex].GetGameObject().SetActive(false);
        if (_currAtkIndex == attacks.Length - 1)
        {
            OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
        } else
        {
            ++_currAtkIndex;
            attacks[_currAtkIndex].GetGameObject().SetActive(true);
        }

    }

    public override void Interrupt()
    {
        attacks[_currAtkIndex].Interrupt();
        DefaultInterrupt();
    }
}