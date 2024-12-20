using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class AbstractMoveAttack : AbstractAttack
{
    [Header("MoveAttack")]
    [Tooltip("The attacker to move")]
    public Rigidbody2D attackerBody;
    [Tooltip("An event that is called when the movement wishes for a shooter to shoot. This is only used for move attacks which need to be precisely synchronised with shootings, so it won't usually be used")]
    public UnityEvent<int> ShootTrigger = new();
}
