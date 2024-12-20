using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wait : AbstractAttack
{
    [Header("Wait")]
    [Tooltip("The time to wait for")]
    [SerializeField] private float waitTime;

    Coroutine waitCouroutine;

    private IEnumerator WaitToDeactivate()
    {
        yield return new WaitForSeconds(waitTime);
        OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
    }

    public override void RestartAttack()
    {
        Cleanup();
        waitCouroutine = StartCoroutine(WaitToDeactivate());
    }

    public override void Cleanup()
    {
        if (waitCouroutine != null)
        {
            StopCoroutine(waitCouroutine);
            waitCouroutine = null;
        }
    }
}
