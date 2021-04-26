using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static DefaultInputActions;

public class GameController : IPlayerActions
{
    public enum ActionType
    {
        Forbiden,
        Move,
        Push
    }

    private HashSet<Vector2> ValidInputs = new HashSet<Vector2>(
        new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1,0),
            new Vector2(-1,0),
            new Vector2(0,-1),
        });

    private GameState gameState;
    private Dictionary<Vector2, Transform> boxGameObjects;
    private PlayerController playerController;

    private DefaultInputActions  controlls;

    public GameController(
        LevelConfig levelConfig, 
        Dictionary<Vector2, Transform> boxObjects,
        PlayerController player
    )
    {
        gameState = new GameState(levelConfig);
        boxGameObjects = boxObjects;
        playerController = player;

        controlls = new DefaultInputActions();
        controlls.Player.SetCallbacks(this);
        controlls.Enable();
    }

    /// <summary>
    /// Callback that gets triggered once the player performs movement input on device
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        // TODO: queue player inputs and process them consequentially...do you need this???

        if (context.action.phase == InputActionPhase.Performed)
        {
            var movementDirection = context.action.ReadValue<Vector2>(); ;
            var actionType = DecodeInputAction(movementDirection);

            if (actionType == ActionType.Forbiden) return;

            // disable input until the current move is finished
            controlls.Disable();

            if (actionType == ActionType.Move)
            {
                playerController.ExecuteAction(actionType, movementDirection, PlayerActionFinishedCallback);
            }
            else if (actionType == ActionType.Push)
            {
                playerController.ExecuteAction(
                    actionType,
                    movementDirection,
                    PlayerActionFinishedCallback,
                    boxGameObjects[gameState.PlayerPosition + movementDirection]);
            }
        }
    }

    /// <summary>
    /// Checks if the intended action is valid and generates action type based on input
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>Action type that needs to be performed</returns>
    private ActionType DecodeInputAction(Vector2 direction)
    {
        Vector2 destination = gameState.PlayerPosition + direction;

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

    /// <summary>
    /// Once the "visual" part of action has done executing, update state before next input
    /// </summary>
    /// <param name="actionType"></param>
    /// <param name="direction"></param>
    private void PlayerActionFinishedCallback(ActionType actionType, Vector2 direction)
    {
        var inputDirection = new Vector2Int((int)direction.x, (int)direction.y);
        
        gameState.MovePlayer(inputDirection);

        // TODO: move to method if it becomes complex
        if (actionType == ActionType.Push)
        {
            gameState.MoveBox(gameState.PlayerPosition, inputDirection);

            var boxPosition = gameState.PlayerPosition + inputDirection;
            boxGameObjects.Add(boxPosition, boxGameObjects[gameState.PlayerPosition]);
            boxGameObjects.Remove(gameState.PlayerPosition);

            if(gameState.BoxHolders.Contains(boxPosition))
            {
                boxGameObjects[boxPosition].GetComponent<SpriteRenderer>().color = new Color(0.2039216f, 1f, 0.3843137f);
            }
            else
            {
                boxGameObjects[boxPosition].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        CheckFinalGoal();
    }

    private void CheckFinalGoal()
    {
        if(gameState.FinalGoalReached)
        {

        }
        else
        {
            controlls.Enable();
        }
    }
}
