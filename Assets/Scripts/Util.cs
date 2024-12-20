using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Util : MonoBehaviour
{
    private Util() { throw new System.Exception("Do not instantiate the Util class"); }

    /*
     * check if the position is within some axial distance of the camera
     */
    public static bool CheckIfInCameraBounds(Vector3 position, float tolerance)
    {
        Vector2 middle = Camera.main.transform.position;
        float sideOffset = Camera.main.orthographicSize * Camera.main.aspect;

        float rightBound = middle.x + sideOffset + tolerance;
        float leftBound = middle.x - sideOffset - tolerance;
        float topBound = middle.y + Camera.main.orthographicSize + tolerance;
        float bottomBound = middle.y - Camera.main.orthographicSize - tolerance;

        //Debug.Log($"cameraPos {Camera.main.transform.position} currPos {transform.position}, horizontalBounds {rightBound}, {leftBound}, verticalBounds {topBound}, {bottomBound}");

        return position.x > rightBound ||
            position.x < leftBound ||
            position.y > topBound ||
            position.y < bottomBound;
    }

    public static void DrawBounds(float bounds)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Camera.main.transform.position, new Vector3(Camera.main.orthographicSize * Camera.main.aspect * 2 + bounds * 2, Camera.main.orthographicSize * 2 + bounds * 2, 1));
    }
}
