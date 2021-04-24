using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public HashSet<Vector2Int> Walkables { get; }
    public HashSet<Vector2Int> BoxHolders { get; }
    public HashSet<Vector2Int> Boxes { get; private set; }
    public Vector2Int PlayerPosition { get; private set; }

    public GameState(LevelConfig levelConfig)
    {

    }

    public void MoveBox(Vector2Int from, Vector2Int to)
    {
        Boxes.Remove(from);
        Boxes.Add(to);
    }

    public void MovePlayer(Vector2Int to)
    {
        PlayerPosition = to;
    }
}
