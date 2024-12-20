using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("AttackManager")]
    [Tooltip("Sets of attacks that are (currently) run through sequentially")]
    public AttackSet[] attackSets;
    //public bool[] randomize;
    private int currAtkSetIdx;
    private int currAtkWithinSet;

    private void Awake()
    {
        foreach (AttackSet attackSet in attackSets)
        {
            foreach (IAttack atk in attackSet.attacks)
            {
                atk.GetGameObject().SetActive(false);
            }
        }
        currAtkSetIdx = 0;
        currAtkWithinSet = 0;
        //if (randomize.Length != attackSets.Length)
        //{
        //    throw new ArgumentException($"randomize is not the same length as attackSets in attackManager {name}");
        //}
    }

    private void Start()
    {
        StartAttacks();
    }

    private IAttack[] CurrAtkSet()
    {
        return attackSets[currAtkSetIdx].attacks;
    }

    public void StartAttacks()
    {
        CurrAtkSet()[currAtkWithinSet].GetDoneEvent().AddListener(OnAttackFinished);
        CurrAtkSet()[currAtkWithinSet].GetGameObject().SetActive(true);
    }

    public void OnAttackFinished(IAttack.AttackContext attackFinishedContext)
    {
        CurrAtkSet()[currAtkWithinSet].GetDoneEvent().RemoveListener(OnAttackFinished);
        CurrAtkSet()[currAtkWithinSet].GetGameObject().SetActive(false);
        ++currAtkWithinSet;
        if (currAtkWithinSet >= CurrAtkSet().Length)
        {
            currAtkWithinSet = 0;
        }
        CurrAtkSet()[currAtkWithinSet].GetDoneEvent().AddListener(OnAttackFinished);
        CurrAtkSet()[currAtkWithinSet].GetGameObject().SetActive(true);
    }

    public void ChangeAttackSet(int newSetIdx)
    {
        Interrupt();
        currAtkSetIdx = newSetIdx;
    }

    public void Interrupt()
    {
        CurrAtkSet()[currAtkWithinSet].Interrupt();
    }

}

[System.Serializable]
public struct AttackSet
{
    [SerializeField]
    public AbstractAttack[] attacks;
}