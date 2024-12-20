using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class AbstractShootAttack : AbstractAttack
{
    // honestly this is pretty bad practice, since this class
    // is designed to handle both random and repeat modes
    // for bullet distributions, however, this will make it
    // easier to edit and switch between in the inspector

    /*
     * all projectiles must have a projectile component
    */
    [Header("Projectile Specifications")]
    [Tooltip("the projectile prefabs to use for shooting. They are spawned using the corresponding weight in weights, so these lists should be the same length")]
    [SerializeField] protected GameObject[] projectiles;
    [Tooltip("please don't put 0 in for any of the weights, that would likely make things not work. More explanation for how these weights work under randomlyChooseProjectile")]
    [SerializeField] protected int[] weights;
    [Tooltip("whether projectiles are chosen by randomly generating or repeating as many times as specified in the weights")]
    [SerializeField] protected bool randomlyChooseProjectile;
    [Header("ShootAttack")]
    [Tooltip("If a movement is provided, the shooting object will be controlled by the movement. This is useful for patterns which require the shooting to be synced up with the movement")]
    [SerializeField] protected AbstractMoveAttack movementThatControlsShooting = null;
    [Tooltip("The object from which to spawn the bullets at")]
    [SerializeField] public GameObject attacker;
    [Tooltip("The Z that the projectile spawns at")]
    [SerializeField] protected float spawnZ = -2;
    [Tooltip("Whether the projectile attempts to spawn at the center of the attacker. The unchecked version will only work if the attacker has a circleCollider2d. When unchecked, the object will try to spawn close to the edge of the attacker's radius, modifying its position by approxProjectileRadius to prevent it from peaking out from under the attacker.")]
    [SerializeField] protected bool spawnAtAttackerCenter = true;
    //[Tooltip("The amount to move backwards from the attacker's edge. Only used if the previous value is false")]
    //[SerializeField] protected float p_approxProjectileRadius = 0.25f;

    protected int currIndex;
    protected int currRepeat;
    protected int weightsSum;

    // cached value
    protected CircleCollider2D attackerCircleCollider;
    /*
     * Shot number is 0 indexed, meaning that the first shot will be shot when
     * the shotNumber passed in is 0, and the 2nd one when shotNumber is 2
     */
    public abstract void Shoot(int shotNumber);
    protected virtual void Awake()
    {
        attackerCircleCollider = attacker.GetComponent<CircleCollider2D>();
        if (projectiles == null || weights == null)
        {
            throw new System.Exception(gameObject.name + " does not have projectiles or weights");
        }
        else if (projectiles.Length != weights.Length)
        {
            throw new System.Exception(gameObject.name + " weights and projectile arrays are not the same length");
        }
        // honestly considered having an if statement
        // to control which one gets initialized but
        // it doesn't save any operations and maybe
        // we want to change isRandom at runtime for
        // some reason...
        weightsSum = weights.Sum();
        currIndex = 0;
        currRepeat = 0;

        if (movementThatControlsShooting != null)
        {
            movementThatControlsShooting.ShootTrigger.AddListener(Shoot);
        }
    }

    protected virtual GameObject GetProjectile()
    {
        if (randomlyChooseProjectile)
        {
            int num = Random.Range(0, weightsSum);
            int i = -1;
            do
            {
                ++i;
                num -= weights[i];
            } while (i < weights.Length && num >= 0);
            return projectiles[i];
        }
        else
        {
            GameObject returnProjectile = projectiles[currIndex];

            ++currRepeat;
            if (currRepeat >= weights[currIndex])
            {
                currRepeat = 0;
                ++currIndex;
                if (currIndex >= weights.Length)
                {
                    currIndex = 0;
                }
            }
            return returnProjectile;
        }
    }

    /*
     * create a projectile going in the specified direction
     * Most projectiles will likely need a direction, but some don't, probably
     * so this method only works if the projectile requires a direction
     */
    protected void CreateProjectile(Vector3 projectileMoveVector)
    {
        Vector3 spawnPos = attacker.transform.position;
        spawnPos.z = spawnZ;
        //Debug.Log(spawnPos.y);

        GameObject newProjectile = Instantiate(
                GetProjectile(),
                spawnPos,
                attacker.transform.rotation,
                GameManager.instance.projectiles.transform);
        AbstractProjectile projectileComponent = newProjectile.GetComponent<AbstractProjectile>();
        Asteroid projectileAsteroid = newProjectile.GetComponent<Asteroid>();
        projectileComponent.movementVector = projectileMoveVector;
        if (!spawnAtAttackerCenter && attackerCircleCollider != null)
        {
            float approxRadius = 0;
            AbstractProjectile abstractProjectile = newProjectile.GetComponent<AbstractProjectile>();
            if (abstractProjectile != null)
            {
                approxRadius = abstractProjectile.approximateRadius;
            }

            Vector3 projectileMoveVectorNormalized = ((Vector2)projectileMoveVector).normalized;
            newProjectile.transform.position += (attackerCircleCollider.radius * attacker.transform.lossyScale.x - approxRadius) * projectileMoveVectorNormalized;
        }

        newProjectile.SetActive(true);

        projectileComponent.damager.AddToDamageDescription($"{SceneManager.GetActiveScene().name}:{name}");
        if (projectileAsteroid != null)
        {
            projectileAsteroid.asteroidTagsBasedDamager.AddToDamageDescription($"{SceneManager.GetActiveScene().name}:{name}");
        }
    }
}