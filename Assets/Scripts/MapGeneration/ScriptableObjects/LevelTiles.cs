using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "LevelTiles", menuName = "Sozap Test/Level Tileset", order = 1)]
public class LevelTiles : ScriptableObject
{
    public Tile Wall;
    public Tile Walkable;
    public Tile BoxHolder;
}
