using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class DrawCircle : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void UpdateDraw(int steps, float radius, Vector2 center)
    {
        lineRenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; ++currentStep)
        {
            float circumferenceProgress = (float) currentStep / (steps - 1);
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            lineRenderer.SetPosition(currentStep, center + new Vector2(x, y));
        }
    }

    public void Clear()
    {
        lineRenderer.enabled = false;
    }

    public void TurnOn()
    {
        lineRenderer.enabled = true;
    }
}
