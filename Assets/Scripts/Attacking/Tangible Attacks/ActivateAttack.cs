using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveAttack : AbstractAttack
{
    [Header("ActiveAttack")]
    [Tooltip("The gameObject to set")]
    [SerializeField] private GameObject activeObject;
    [Tooltip("what state of active should the object be in by default (false - inactive, true - active)")]
    [SerializeField] private bool activeOnLoad = false;
    [Tooltip("The time to change it for")]
    [SerializeField] private float timeActive;

    Coroutine waitCouroutine;

    private IEnumerator WaitToDeactivate()
    {
        yield return new WaitForSeconds(timeActive);
        activeObject.SetActive(activeOnLoad);
        OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
    }

    public override void RestartAttack()
    {
        Cleanup();
        activeObject.SetActive(!activeOnLoad);
        waitCouroutine = StartCoroutine(WaitToDeactivate());
    }

    public override void Cleanup()
    {
        if (waitCouroutine != null)
        {
            StopCoroutine(waitCouroutine);
            activeObject.SetActive(activeOnLoad);
            waitCouroutine = null;
        }
    }
}
