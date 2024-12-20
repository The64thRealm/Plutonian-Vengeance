using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DistanceJoint2D))]
[RequireComponent (typeof(DrawCircle))]
[RequireComponent(typeof(TagsBasedDamager))]
public class Asteroid : MonoBehaviour
{
    public UnityEvent<Asteroid> OnAsteroidCaptured = new();
    public UnityEvent<Asteroid> OnAsteroidUncaptured = new();

    private bool hasBeenCapturedBefore;
    private DistanceJoint2D orbitJoint;
    private DrawCircle circle;
    public TagsBasedDamager asteroidTagsBasedDamager { get; private set; }

    public bool isLockedInOrbit { get; private set; }
    private float orbitDistance;
    private AbstractProjectile projectile;

    [Tooltip("The gameObject with a collider that does damage to extra things - used for blowing through enemy asteroids")]
    public GameObject BonusDamager;

    private void Awake()
    {
        BonusDamager.SetActive(false);

        if (asteroidTagsBasedDamager == null)
        {
            recognizeTagsBasedDamager();
        }
        hasBeenCapturedBefore = false;
        projectile = GetComponent<AbstractProjectile>();
        orbitJoint = GetComponent<DistanceJoint2D>();
        circle = GetComponent<DrawCircle>();
        orbitJoint.connectedBody = GameManager.instance.pluto.GetComponent<Rigidbody2D>();
        orbitJoint.enabled = false;

        asteroidTagsBasedDamager.GetDealtDamageEvent().AddListener(TrackAsteroidDamage);
        asteroidTagsBasedDamager.AddToDamageDescription("");
    }

    private void Start()
    {
        GameManager.instance.AsteroidCreated(this, asteroidTagsBasedDamager.damageDescription);
    }

    public void SetLockedInOrbit(bool newIsLockedInOrbit = true, Rigidbody2D connectedBody = null)
    {
        if (newIsLockedInOrbit && !this.isLockedInOrbit)
        {
            // stuff to do on first capture
            if (!hasBeenCapturedBefore)
            {
                GameManager.instance.AsteroidCaptured(this, asteroidTagsBasedDamager.damageDescription);
                gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                gameObject.tag = "Asteroid_Friendly";
                hasBeenCapturedBefore = true;
                projectile.enabled = false;
            }

            // stuff to do always
            OnAsteroidCaptured?.Invoke(this);
            circle.TurnOn();
            // don't want to account for z distance when making this value
            orbitDistance = ((Vector2) (transform.position - GameManager.instance.pluto.transform.position)).magnitude;
            orbitJoint.distance = orbitDistance;
            asteroidTagsBasedDamager.enabled = true;
        }
        else if (!newIsLockedInOrbit && this.isLockedInOrbit)
        {
            OnAsteroidUncaptured?.Invoke(this);
            circle.Clear();
        }

        if (connectedBody != null)
        {
            orbitJoint.connectedBody = connectedBody;
        }
        orbitJoint.enabled = newIsLockedInOrbit;
        this.isLockedInOrbit = newIsLockedInOrbit;
    }

    public void ChangeToBonusLayer()
    {
        if (hasBeenCapturedBefore)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerBonusProjectile");
            BonusDamager.SetActive(true);
        }
    }

    public void RemoveFromBonusLayer()
    {
        BonusDamager.SetActive(false);
        if (hasBeenCapturedBefore)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        } else
        {
            gameObject.layer = LayerMask.NameToLayer("Projectile");
        }
    }

    private void LateUpdate()
    {
        if (isLockedInOrbit)
        {
            circle.UpdateDraw(50, orbitDistance, GameManager.instance.pluto.transform.position);
            if (Util.CheckIfInCameraBounds(transform.position, AbstractProjectile.KILLZONE_BOUNDS_OFFSET)) { Destroy(gameObject); }
        }
    }
    private void FixedUpdate()
    {
        if (isLockedInOrbit)
        {
            // don't want the connected body to experience any forces from the joint
            Vector2 counteractionForce = orbitJoint.reactionForce;
            if (float.IsNaN(counteractionForce.x)) { counteractionForce.x = 0; }
            if (float.IsNaN(counteractionForce.y)) { counteractionForce.y = 0; }
            orbitJoint.connectedBody.AddForce(-counteractionForce, ForceMode2D.Force);
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.capturableAsteroids.Remove(this);
        GameManager.instance.pluto.plutoAsteroidCaptureHandler.AsteroidRemoved(this);
        OnAsteroidUncaptured?.Invoke(this);
        asteroidTagsBasedDamager.GetDealtDamageEvent().RemoveListener(TrackAsteroidDamage);
    }

    private void OnDrawGizmosSelected()
    {
        Util.DrawBounds(AbstractProjectile.KILLZONE_BOUNDS_OFFSET);
    }

    private void TrackAsteroidDamage(IDamager.Damage damageContext)
    {
        GameManager.instance.AsteroidCollidedWithPlanet(damageContext.attackStr, damageContext.damageAmount);
    }

    public void recognizeTagsBasedDamager()
    {
        TagsBasedDamager[] damagers = GetComponents<TagsBasedDamager>();
        if (damagers.Length < 2)
        {
            throw new System.Exception("Please add more than one TagsBasedDamager to the capturable asteroid, the first one specifying the original damaging tags, and the second specifying the asteroid damaging tags");
        }
        asteroidTagsBasedDamager = damagers[1];
        asteroidTagsBasedDamager.enabled = false;
    }
}