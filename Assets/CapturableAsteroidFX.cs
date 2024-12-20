using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturableAsteroidFX : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void EnteredOrbit()
    {
        animator.SetBool("InOrbit", true);
    }

    public void ExitedOrbit()
    {
        animator.SetBool("InOrbit", false);
    }

    public void OnFire()
    {
        animator.SetBool("OnFire", true);
    }
    public void ExtinguishFire()
    {
        animator.SetBool("OnFire", false);
    }
}
