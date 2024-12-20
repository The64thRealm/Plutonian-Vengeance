using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedHealthCheckpointHandler : MonoBehaviour
{
    [Tooltip("Must be in descending order")]
    public float[] healthCheckpoints;
    public int healthCheckpointsReached { get; private set; }


    
    private void Start()
    {
        //TODO sort the array in descending
    }
    public void HandleEnemyDamaged(IDamageable.DamagedInfo damagedInfo)
    {
        while (damagedInfo.resultantHealth < healthCheckpoints[healthCheckpoints.Length - healthCheckpointsReached - 1])
        {
            ++healthCheckpointsReached;
            CheckCheckpoints(damagedInfo);
        }
    }

    public virtual void CheckCheckpoints(IDamageable.DamagedInfo damagedInfo)
    {
        switch (healthCheckpointsReached)
        {
            case 1:
                // Do what happens on the first checkpoint reached here
                break;
            case 2:
                // Do what happens on the second checkpoint reached here
                break;
            case 3:
                // Do what happens on the third checkpoint reached here
                break;
            default:
                // Do what happens on all other checkpoints here
                break;
        }
    }
}
