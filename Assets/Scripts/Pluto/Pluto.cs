using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Pluto : MonoBehaviour
{

    public PlutoInputHandler plutoInputHandler { get; private set; }
    public PlutoAsteroidCaptureHandler plutoAsteroidCaptureHandler { get; private set; }
    public PlutoMovement plutoMovement { get; private set; }
    public HealthCheck plutoHealthCheck { get; private set; }

    private Animator animator;

    private void Awake()
    {
        plutoMovement = GetComponent<PlutoMovement>();
        plutoHealthCheck = GetComponent<HealthCheck>();
        plutoAsteroidCaptureHandler = GetComponent<PlutoAsteroidCaptureHandler>();
        plutoInputHandler = GetComponent<PlutoInputHandler>();
        animator = GetComponent<Animator>();
        plutoHealthCheck.OnDeath.AddListener(OnPlutoDeath);
    }

    public void StartDeathAnimation()
    {
        plutoAsteroidCaptureHandler.ReleaseAsteroids();
        animator.SetTrigger("OnDeath");
        Destroy(plutoHealthCheck.healthBar);  
    }

    public void DeathAnimationEnded()
    {
        Destroy(plutoHealthCheck.healthBar);
        SceneManager.LoadScene("DeathScreen");
    }

    public void OnPlutoDeath(IDamageable.DamagedInfo context)
    {
        plutoMovement.enabled = false;
        plutoAsteroidCaptureHandler?.ReleaseAsteroids();
        plutoInputHandler.playerInputActions.Pluto.Disable();
    }
}
