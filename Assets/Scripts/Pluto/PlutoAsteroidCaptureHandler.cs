using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.ComponentModel;

public class PlutoAsteroidCaptureHandler : MonoBehaviour
{
    [Tooltip("The furthest away that a capturable asteroid can be where pluto can still capture it")]
    [SerializeField] private float maxOrbitalDistance = 5f;
    public UnityEvent OnAsteroidCaputured;

    [SerializeField] private ParticleSystem attractParticleSystem;

    [Header("Pluto attraction cooldown values")]
    [Tooltip("The longest time in seconds that pluto can attract for")]
    [SerializeField] private float maxDuration = 5f;
    [Tooltip("The amount of time that pluto will take to fully recover its \"attact meter\" from 0")]
    [SerializeField] private float totalCooldownTime = 5f;
    [Tooltip("The amount of time to wait before starting to recover pluto's \"attract meter\" again")]
    [SerializeField] private float minimumCooldownTime = 1f;
    [Tooltip("Once the \"attract meter\" is below this percentage, pluto will no longer be able to attract until the amount recovers above this percentage")]
    [SerializeField, Range(0, 1)] private float cutoffPercentage = 0.3f;
    
    private List<Asteroid> orbitingAsteroids;
    private Pluto pluto;

    [Header("Particle System")]
    [SerializeField] private Color initialColor = Color.white;
    [SerializeField] private Color meterLowColor = Color.red;

    public UnityEvent OnAttract = new();
    public UnityEvent OnRelease = new();

    [Header("These values are not meant to be set, only read for debugging")]
    [SerializeField, Range(0, 1)] private float attractMeterPercentage;

    // these values are how much percent per second to decrease/increase the meter by when being used/not used
    [SerializeField] private float decayPercentage;
    [SerializeField] private float recoveryPercentage;

    // time left until we can start recovering
    [SerializeField] private float recoveryCooldown;

    // input handling
    [SerializeField] private bool inputHeld;
    [SerializeField] private bool attracting;


    private ParticleSystem.MainModule particleMain;

    private void Awake()
    {
        inputHeld = false;
        attracting = false;
        attractMeterPercentage = 1;
        CalculateDecayAndRecoverPercentages();
        pluto = GetComponent<Pluto>();
        orbitingAsteroids = new List<Asteroid>();
        particleMain = attractParticleSystem.main;
        if (attractParticleSystem.shape.radius != maxOrbitalDistance)
        {
            Debug.LogWarning("The max orbital radius and the particle system do not have the same radius, are you sure this is what you wanted?");
        }
    }

    private void Update()
    {
        if (attracting)
        {
            if (!attractParticleSystem.isPlaying)
            {
                attractParticleSystem.Play();
            }
            CaptureNewAsteroids();
            attractMeterPercentage -= decayPercentage * Time.deltaTime;
            recoveryCooldown = minimumCooldownTime;
            if (attractMeterPercentage <= 0)
            {
                attractMeterPercentage = 0;
                OnRelease?.Invoke();
                ReleaseAsteroids();
            }
            particleMain.startColor = Color.Lerp(meterLowColor, initialColor, attractMeterPercentage);
        } else
        {
            if (attractParticleSystem.isPlaying)
            {
                attractParticleSystem.Stop();
            }
            if (recoveryCooldown <= 0)
            {
                attractMeterPercentage += recoveryPercentage * Time.deltaTime;
                if (attractMeterPercentage >= 1)
                {
                    attractMeterPercentage = 1;
                }
            }
            if (inputHeld && attractMeterPercentage >= cutoffPercentage)
            {
                OnAttract?.Invoke();
                attracting = true;
            }
            recoveryCooldown -= Time.deltaTime;
        }
    }

    public void Attract(InputAction.CallbackContext context)
    {
        inputHeld = true;
    }

    private void CaptureNewAsteroids()
    {
        foreach (Asteroid asteroid in GameManager.instance.capturableAsteroids)
        {
            if (Vector2.Distance(asteroid.transform.position, transform.position) < maxOrbitalDistance)
            {
                orbitingAsteroids.Add(asteroid);
                asteroid.SetLockedInOrbit(true);
            }
        }
    }

    public void Release(InputAction.CallbackContext context)
    {
        if (attracting)
        {
            OnRelease?.Invoke();
            ReleaseAsteroids();
        }
        inputHeld = false;
    }


    public void ReleaseAsteroids()
    {
        attracting = false;
        foreach (Asteroid asteroid in orbitingAsteroids)
        {
            // TODO idk why this if statement is necessary
            if (asteroid)
            {
                asteroid.SetLockedInOrbit(false);
            }
        }
        orbitingAsteroids.Clear();
    }
    
    // TODO probably use an event instead so that we don't call it in asteroids
    // this method is kinda messy and I dislike it, it's used for when the asteroid
    // dies while in orbit and needs to tell this component that it doesn't exist anymore
    public void AsteroidRemoved(Asteroid asteroid)
    {
        orbitingAsteroids.Remove(asteroid);
    }
    private void CalculateDecayAndRecoverPercentages()
    {
        recoveryPercentage = 1 / (totalCooldownTime - minimumCooldownTime);
        decayPercentage = 1 / maxDuration;
    }
}
