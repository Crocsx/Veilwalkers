using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    private PlayerActionMap controls;

    public static InputManager Instance {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
        } 
        else
        {
            _instance = this;
        }
        controls = new PlayerActionMap();
    }

    // Update is called once per frame
    void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public Vector2 GetMovement()
    {
        return controls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetLook()
    {
        return controls.Player.Look.ReadValue<Vector2>();
    }

    public Boolean IsJumpingThisFrame()
    {
        return controls.Player.Jump.triggered;
    }

    public Boolean IsCrouching()
    {
        return controls.Player.Crouch.IsPressed();
    }
}
