using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameplayController
{
    public enum ActionType
    {
        Forbiden,
        Move,
        Push
    }

    public UnityEvent LevelCompleted = new UnityEvent();

    private GameStateData gameState;
    private Dictionary<Vector2, Transform> boxGameObjects;
    private PlayerController playerController;
    private PlayerInput controlls;

    public GameplayController(
        LevelConfigData levelConfig, 
        Dictionary<Vector2, Transform> boxObjects,
        PlayerController player,
        LevelScoreData levelScore,
        PlayerInput playerControlls
    )
    {
        gameState = new GameStateData(levelConfig);
        boxGameObjects = boxObjects;
        playerController = player;

        controlls = playerControlls;
        controlls.SetMovementListener(MovePlayer);
        controlls.Enable();
    }

    /// <summary>
    /// Callback that gets triggered once the player performs movement input on device
    /// </summary>
    /// <param name="context"></param>
    private void MovePlayer(Vector2 movementDirection)
    {   
        var actionType = DecodeInputAction(movementDirection);

        if (actionType == ActionType.Forbiden) return;

        // disable input until the current move is finished
        controlls.Disable();

        if (actionType == ActionType.Move)
        {
            playerController.ExecuteAction(actionType, movementDirection, OnPlayerActionFinished);
        }
        else if (actionType == ActionType.Push)
        {
            playerController.ExecuteAction(
                actionType,
                movementDirection,
                OnPlayerActionFinished,
                boxGameObjects[gameState.PlayerPosition + movementDirection]);
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
    private void OnPlayerActionFinished(ActionType actionType, Vector2 direction)
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
            LevelCompleted?.Invoke();
        }
        else
        {
            controlls.Enable();
        }
    }
}
