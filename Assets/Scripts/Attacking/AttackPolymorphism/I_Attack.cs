using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * If the attack contains members that act sequentially, OnDisable must also disable all children seperately
 * The attack starts when enabled
 * Upon invoking the done event, the parent should either choose to restart the attack or set the attack to disabled
 * (unless otherwise specified by the class, such as LoopingContainer)
 * When the attack ends, it should trigger the done event if ended normally, or the interrupt event if interrupted
 * The attack should only handle disabling itself if interrupted. The parent should be responsible otherwise
 * OnDisabled should be used to clean up anything after the attack finishes
 */
public interface IAttack
{
    public struct AttackContext
    {
        public AttackContext(int timesFinished, IAttack callingAttack)
        {
            this.finishedCount = timesFinished;
            this.callingAttack = callingAttack;
        }
        public IAttack callingAttack;
        public int finishedCount;
    }
    // called when interrupted
    public void Interrupt();
    // resets the finish count of the attack
    public void ResetFinishCount();
    /*
     * resets everything about the attack besides its finish count
     * used to loop the attack after it stops
     * or to reset the attack before starting it, usually you'd want
     * to call resetFinishCount in that situation too
     */
    public void RestartAttack();
    // bad design but whatever
    public GameObject GetGameObject();

    // returns the event that is associated with this attack for finishing
    public UnityEvent<AttackContext> GetDoneEvent();
    // returns the event that is associated with this attack for being interrupted
    public UnityEvent<AttackContext> GetInterruptedEvent();
}