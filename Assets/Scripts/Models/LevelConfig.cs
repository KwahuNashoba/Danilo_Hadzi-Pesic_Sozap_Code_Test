using UnityEngine;

[System.Serializable]
public class LevelConfig
{
    public Vector2Int[] Collision;
    public Vector2Int[] Walkable;
    public Vector2Int[] BoxHolders;
    public Vector2Int[] Boxes;
    public Vector2Int StartPosition;
}
