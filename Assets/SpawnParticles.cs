using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;

    public void CreateParticles()
    {
        Destroy(Instantiate(particlePrefab, transform.position, Quaternion.identity), 2);
    }
}
