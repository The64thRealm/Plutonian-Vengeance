using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    public Vector2 offset = new(0, 0.25f);
    public GameObject objectDestination;

    // Update is called once per frame
    void Update()
    {
        transform.position = objectDestination.transform.position + (Vector3)offset;
    }
}
