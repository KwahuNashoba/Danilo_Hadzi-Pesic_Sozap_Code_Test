using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static DefaultInputActions;

public class PlayerInput : IPlayerActions
{
    private HashSet<Vector2> ValidInputs = new HashSet<Vector2>(
           new Vector2[]
           {
            new Vector2(0, 1),
            new Vector2(1,0),
            new Vector2(-1,0),
            new Vector2(0,-1),
           });

    private DefaultInputActions controlls;
    private Action<Vector2> movementInputCallback;

    public PlayerInput()
    {
        InitializeControlls();
        controlls.Player.SetCallbacks(this);
    }

    public void SetMovementListener(Action<Vector2> movementCallback)
    {
        movementInputCallback = movementCallback;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // TODO: queue player inputs and process them consequentially...do you need this???
        if (context.action.phase == InputActionPhase.Performed)
        {
            var movementDirection = context.action.ReadValue<Vector2>();
            if(ValidInputs.Contains(movementDirection))
            {
                movementInputCallback?.Invoke(movementDirection);
            }
        }
    }

    public void Enable()
    {
        controlls.Enable();
    }

    public void Disable()
    {
        controlls.Disable();
    }

    private void InitializeControlls()
    {
        if (controlls == null)
        {
            controlls = new DefaultInputActions();
        }
    }
}
