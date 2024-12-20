using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChooseGroup : AbstractAttackGroup
{
    [SerializeField] private int timesToChoose = 10;

    private int currentTimesChosen;
    private int currentIndexChosen;

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
        currentTimesChosen = 0;
        currentIndexChosen = RandomlyChooseAttackIndex();
        attacks[currentIndexChosen].GetGameObject().SetActive(true);
    }

    public void OnChildAttackFinished(IAttack.AttackContext context)
    {
        ++currentTimesChosen;
        if (currentTimesChosen >= timesToChoose)
        {
            OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
        }
        else
        {
            attacks[currentIndexChosen].GetGameObject().SetActive(false);
            currentIndexChosen = RandomlyChooseAttackIndex();
            attacks[currentIndexChosen].GetGameObject().SetActive(true);
        }
    }

    public override void Interrupt()
    {
        attacks[currentIndexChosen].Interrupt();
        DefaultInterrupt();
    }

    protected virtual int RandomlyChooseAttackIndex()
    {
        return Random.Range(0, attacks.Length);
    }
}
