using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static DefaultInputActions;

public class GameController : IPlayerActions
{
    private enum ActionType
    {
        Forbiden,
        Move,
        Push
    }

    private HashSet<Vector2Int> ValidInputs = new HashSet<Vector2Int>(
        new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
        });

    private GameState gameState;
    private Dictionary<Vector2Int, Transform> boxGameObjects;
    private PlayerController playerController;

    public GameController(
        LevelConfig levelConfig, 
        Dictionary<Vector2Int, Transform> boxObjects,
        PlayerController player
    )
    {
        gameState = new GameState(levelConfig);
        boxGameObjects = boxObjects;
        playerController = player;

        var controlls = new DefaultInputActions();
        controlls.Player.SetCallbacks(this);
        controlls.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // TODO: queue player inputs and process them consequentially
        if (context.action.phase == InputActionPhase.Performed)
        {
            ProcessPlayerInput(context.action.ReadValue<Vector2>());
        }
    }

    private void ProcessPlayerInput(Vector2 playerInput)
    {
        var inputDirection = new Vector2Int((int)playerInput.x, (int)playerInput.y);
        if (ValidateInput(inputDirection) == ActionType.Move)
        {
            playerController.Move(playerInput);
            gameState.MovePlayer(inputDirection);
        }
        else if(ValidateInput(inputDirection) == ActionType.Push)
        {
            playerController.Push(playerInput, boxGameObjects[gameState.PlayerPosition + inputDirection]);
            gameState.MovePlayer(inputDirection);
            gameState.MoveBox(gameState.PlayerPosition, inputDirection);
        }
    }

    private ActionType ValidateInput(Vector2Int direction)
    {
        Vector2Int destination = gameState.PlayerPosition + direction;

        // check if the input direction is valid, at first
        if(!ValidInputs.Contains(direction))
        {
            return ActionType.Forbiden;
        }

        if (gameState.Collision.Contains(destination))
        {
            return ActionType.Forbiden;
        }
        else
        {
            if(gameState.Boxes.Contains(destination))
            {
                var boxDestination = destination + direction;
                if (!gameState.Collision.Contains(boxDestination))
                {
                    if(gameState.Boxes.Contains(boxDestination))
                    {
                        return ActionType.Forbiden;
                    }
                    else
                    {
                        return ActionType.Push;
                    }
                }
                else
                {
                    return ActionType.Forbiden;
                }
            }
            else
            {
                return ActionType.Move;
            }
        }
    }
}
