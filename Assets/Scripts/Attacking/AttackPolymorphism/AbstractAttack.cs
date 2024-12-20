using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * RestartAttack() - override this with everything that needs to be run to set up the attack
 * Update or a coroutine should be responsible for running the attack
 * When the attack is finished, update or coroutine should invoke the OnFinished event.
 * Finished count will be kept track of automatically in this class
 * Cleanup() - override this with everything that needs to be run to cleanup the attack, like deleting coroutines
 * 
 * OnEnabled and OnDisabled will probably never need to be overridden
 */
public abstract class AbstractAttack : MonoBehaviour, IAttack
{
    [Header("Attack Events")]
    [Tooltip("Called when the attack finishes (but not interrupted)")]
    public UnityEvent<IAttack.AttackContext> OnFinished = new();
    [Tooltip("Called when the attack is interrupted.")]
    public UnityEvent<IAttack.AttackContext> OnInterrupt = new();
    protected int finishedCount;

    public UnityEvent<IAttack.AttackContext> GetDoneEvent()
    {
        return OnFinished;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public UnityEvent<IAttack.AttackContext> GetInterruptedEvent()
    {
        return OnInterrupt;
    }

    public virtual void Interrupt()
    {
        OnInterrupt?.Invoke(new IAttack.AttackContext(finishedCount, this));
        Cleanup();
        gameObject.SetActive(false);
    }

    public void ResetFinishCount()
    {
        // add a listener if it isn't there
        OnFinished.RemoveListener(incrementFinishedCounter);
        OnFinished.AddListener(incrementFinishedCounter);
        // finishedCount is reset to 1 because we don't actually increment the count
        // until the event is called
        finishedCount = 1;
    }

    /*
     * whatever is in the Update method should start automatically running the attack
     */
    protected virtual void OnEnable()
    {
        RestartAttack();
        ResetFinishCount();
    }

    protected virtual void OnDisable()
    {
        Cleanup();
    }

    public virtual void Cleanup() {

    }
    public abstract void RestartAttack();
    
    public void incrementFinishedCounter(IAttack.AttackContext context)
    {
        ++finishedCount;
    }
}
