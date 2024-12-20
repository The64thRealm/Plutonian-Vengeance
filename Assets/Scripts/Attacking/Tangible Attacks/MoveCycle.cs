using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public Vector2 direction = Vector2.right;
    public float speed = 1f;
    public int size = 1;

    private Vector2 topEdge;
    private Vector2 bottomEdge;

    private void Start()
    {
        topEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        bottomEdge = Camera.main.ViewportToWorldPoint(Vector3.up);
    }

    private void Update()
    {
        if (direction.y > 0 && (transform.position.y - size) > bottomEdge.y)
        {
            Vector2 position = transform.position;
            position.y = topEdge.y - size;
            transform.position = position;
        }
        else if (direction.y     > 0 && (transform.position.y + size) < topEdge.y)
        {
            Vector2 position = transform.position;
            position.y = bottomEdge.y + size;
            transform.position = position;
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
