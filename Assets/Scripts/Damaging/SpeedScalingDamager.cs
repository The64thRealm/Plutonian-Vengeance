using UnityEngine;

[Tooltip("The damageToDo value is now the base damage")]
public class SpeedScalingCollisionDamager : CollisionDamager
{
    [Header("SpeedScalingCollisionDamager")]
    [Tooltip("The amount of extra damage for having high speed. Damage is calculated with speedDamageBonusCoefficient * min(speed, maximumSpeedForBonus) + damageToDo")]
    [SerializeField] private float speedDamageBonusCoefficient = 0.25f;
    [Tooltip("A cap on the maximum velocity that can be used for this bonus")]
    [SerializeField] private float maximumSpeedForBonus = 28;

    protected Collision2D recentCollision { get; set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        recentCollision = collision;
        base.OnCollisionEnter2D(collision);
    }

    public override float CalculateDamage()
    {
        //Debug.Log(recentCollision.relativeVelocity.magnitude);
        return speedDamageBonusCoefficient * Mathf.Min(recentCollision.relativeVelocity.magnitude, maximumSpeedForBonus) + damageToDo; 
    }
}