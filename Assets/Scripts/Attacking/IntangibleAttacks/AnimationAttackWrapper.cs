using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Tooltip("This class may not work with looping. Connect the animation start method to the OnAttackStartEvent, the reset to the interrupted, and add an animation event that calls animationDone")]
public class AnimationAttackWrapper : AbstractAttack
{
    [Header("AnimationAttackWrapper")]
    [Tooltip("Connect the animation start to this method unless the animation starts when the current object is enabled")]
    public UnityEvent OnAttackStart = new();
    public override void RestartAttack() {
        OnAttackStart?.Invoke();
    }

    public void AnimationDone()
    {
        OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
    }
}
