using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAt : AbstractShootAttack
{
    [Header("ShootAt")]
    [Tooltip("the object to shoot at. If no object to shoot at is specified, will shoot in the direction instead")]
    [SerializeField] GameObject target;
    [Tooltip("whether the direction to shoot in should be random for every shot or not")]
    [SerializeField] bool randomizeDirection;
    [Tooltip("if the direction is not random, then will shoot in the specified direction")]
    [SerializeField] Vector2 direction;
    [Tooltip("how long in seconds should be waited between shots")]
    [SerializeField] float timeInterval;
    [Tooltip("speed that the projectile spawns moving at")]
    [SerializeField] float projectileSpeed = 1;
    [Tooltip("must be an odd number to guarantee symmetry")]
    [SerializeField] int numProjectiles = 1;
    [Tooltip("The degrees that each projectile is spread apart from another projectile")]
    [SerializeField] float degreeSpread = 30;


    private int numberOfTimesShot;
    private Coroutine shootCoroutine;

    public override void Shoot(int shotNumber)
    {
        Vector2 normalizedDir = getNormalizedDirection();
        if (numProjectiles % 2 == 1)
        {
            CreateProjectile(normalizedDir * projectileSpeed);
            for (int i = 1; i <= numProjectiles / 2; ++i)
            {
                CreateProjectile(Quaternion.AngleAxis(degreeSpread * i, Vector3.forward) * (normalizedDir * projectileSpeed));
                CreateProjectile(Quaternion.AngleAxis(-degreeSpread * i, Vector3.forward) * (normalizedDir * projectileSpeed));
            }
        }
        else
        {
            for (int i = 0; i < numProjectiles / 2; ++i)
            {
                CreateProjectile(Quaternion.AngleAxis(degreeSpread * i + degreeSpread / 2, Vector3.forward) * (normalizedDir * projectileSpeed));
                CreateProjectile(Quaternion.AngleAxis(-degreeSpread * i - degreeSpread / 2, Vector3.forward) * (normalizedDir * projectileSpeed));
            }
        }
        ++numberOfTimesShot;
    }

    protected Vector2 getNormalizedDirection()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - attacker.transform.position;
            direction.z = 0;
            return direction.normalized;
        } else if (randomizeDirection)
        {
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            return new Vector2(
                Mathf.Cos(randomAngle),
                Mathf.Sin(randomAngle)
                );
        }
        else
        {
            return direction.normalized;
        }
    }

    public IEnumerator timeShots()
    {
        // not sure if this condition actually does anything
        // it just feels more professional than while true :/
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(timeInterval);
            Shoot(numberOfTimesShot);
            OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
        }
    }

    public override void RestartAttack()
    {
        Cleanup();
        shootCoroutine = StartCoroutine(timeShots());
    }

    public override void Cleanup()
    {
        if(shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }
}
