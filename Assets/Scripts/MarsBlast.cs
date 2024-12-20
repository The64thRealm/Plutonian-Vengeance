using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsBlast : MonoBehaviour
{
    Collider2D blast;
    private void Awake()
    {
        blast = GetComponent<Collider2D>();
    }
    private void Start()
    {
        blast.enabled = false;
    }

    public void BlastOn()
    {
        blast.enabled = true;
    }

    public void BlastOff()
    {
        blast.enabled = false;
    }
}
