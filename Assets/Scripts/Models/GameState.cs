using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public HashSet<Vector2Int> Collision { get; }
    public HashSet<Vector2Int> BoxHolders { get; }
    public HashSet<Vector2Int> Boxes { get; private set; }
    public Vector2Int PlayerPosition { get; private set; }

    public GameState(LevelConfig levelConfig)
    {
        Collision = new HashSet<Vector2Int>(levelConfig.Collision);
        BoxHolders = new HashSet<Vector2Int>(levelConfig.BoxHolders);
        Boxes = new HashSet<Vector2Int>(levelConfig.Boxes);
        PlayerPosition = levelConfig.StartPosition;
    }

    public void MoveBox(Vector2Int from, Vector2Int direction)
    {
        Boxes.Remove(from);
        Boxes.Add(from + direction);
    }

    public void MovePlayer(Vector2Int direction)
    {
        PlayerPosition += direction;
    }
}
