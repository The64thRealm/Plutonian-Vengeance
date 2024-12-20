using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineProjectile : AbstractProjectile
{
    private void Start()
    {
        if (faceMovementDirection)
        {
            gameObject.transform.right = movementVector;
        }
        projectileBody.velocity = movementVector;
    }
}
