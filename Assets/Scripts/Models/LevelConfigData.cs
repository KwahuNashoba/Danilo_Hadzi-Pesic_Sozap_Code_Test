using UnityEngine;

[System.Serializable]
public class LevelConfigData
{
    public string LevelId;
    public Vector2[] Collision;
    public Vector2[] Walkable;
    public Vector2[] BoxHolders;
    public Vector2[] Boxes;
    public Vector2 StartPosition;
}
