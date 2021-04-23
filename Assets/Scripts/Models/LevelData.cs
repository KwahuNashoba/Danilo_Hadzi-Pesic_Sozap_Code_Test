using UnityEngine;

[System.Serializable]
public class LevelData
{
    public Vector2Int[] Walls;
    public Vector2Int[] Walkable;
    public Vector2Int[] BoxHolders;
    public Vector2Int[] Boxes;
    public Vector2Int StartPosition;
}
