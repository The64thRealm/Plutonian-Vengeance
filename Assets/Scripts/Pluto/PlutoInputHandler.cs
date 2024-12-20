using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlutoInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    public IA_Player playerInputActions { get; private set; }
    private PlutoAsteroidCaptureHandler plutoAsteroidCaptureHandler;

    private void Awake()
    {
        plutoAsteroidCaptureHandler = GetComponent<PlutoAsteroidCaptureHandler>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new();
        playerInputActions.Pluto.Enable();
        BindControls();
    }
    private void OnDestroy()
    {
        UnbindControls();
    }

    private void BindControls()
    {
        playerInputActions.Pluto.Gravity.performed += plutoAsteroidCaptureHandler.Attract;
        playerInputActions.Pluto.Gravity.canceled += plutoAsteroidCaptureHandler.Release;
    }

    private void UnbindControls()
    {
        playerInputActions.Pluto.Gravity.performed -= plutoAsteroidCaptureHandler.Attract;
        playerInputActions.Pluto.Gravity.canceled -= plutoAsteroidCaptureHandler.Release;
    }
}
