using System;
using System.Collections;
using UnityEngine;
using static GameController;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1;

    public void ExecuteAction(
        ActionType actionType, 
        Vector2 actionDirection, 
        Action<ActionType, Vector2> actionFinishedCallback,
        object actionPayload = null)
    {
        switch(actionType)
        {
            case ActionType.Move:
                StartCoroutine(Move(actionDirection, actionFinishedCallback));
                break;
            case ActionType.Push:
                StartCoroutine(Push(actionDirection, actionFinishedCallback, (Transform)actionPayload));
                break;
        }
    }

    private IEnumerator Move(Vector3 direction, Action<ActionType,Vector2> finishCallback)
    {
        var startingPosition = transform.position;
        var destination = transform.position + direction;

        yield return StartCoroutine(LerpPosition(startingPosition, destination, transform, movementSpeed));
        
        finishCallback.Invoke(ActionType.Move, direction);
    }

    private IEnumerator Push(Vector3 direction, Action<ActionType,Vector2> finishCallback, Transform box)
    {
        var startingPosition = transform.position;
        var destination = transform.position + direction;

        StartCoroutine(LerpPosition(startingPosition, destination, transform, movementSpeed));
        yield return StartCoroutine(
            LerpPosition(startingPosition + direction, destination + direction, box.transform, movementSpeed));
        
        finishCallback.Invoke(ActionType.Push, direction);
    }

    private IEnumerator LerpPosition(Vector3 from, Vector3 to, Transform transform, float speed)
    {

        float lerpProgress = 0f;
        while (Mathf.Clamp01(lerpProgress) != 1)
        {
            lerpProgress = (lerpProgress + Time.deltaTime * speed) / 1f;
            transform.position = Vector3.Lerp(from, to, lerpProgress);
            yield return null;
        }

    }
}

