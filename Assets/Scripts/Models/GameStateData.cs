using System.Collections.Generic;
using UnityEngine;

public class GameStateData
{
    public HashSet<Vector2> Collision { get; }
    public HashSet<Vector2> BoxHolders { get; }
    public HashSet<Vector2> Boxes { get; private set; }
    public Vector2 PlayerPosition { get; private set; }
    public bool FinalGoalReached { get { return Boxes.IsSubsetOf(BoxHolders); } }

    public GameStateData(LevelConfigData levelConfig)
    {
        Collision = new HashSet<Vector2>(levelConfig.Collision);
        BoxHolders = new HashSet<Vector2>(levelConfig.BoxHolders);
        Boxes = new HashSet<Vector2>(levelConfig.Boxes);
        PlayerPosition = levelConfig.StartPosition;
    }

    public void MoveBox(Vector2 from, Vector2 direction)
    {
        Boxes.Remove(from);
        Boxes.Add(from + direction);
    }

    public void MovePlayer(Vector2 direction)
    {
        PlayerPosition += direction;
    }
}
