using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CollisionTilesFactory", menuName = "Sozap Test/Collision Tiles Factory")]
public class CollisionTilemapFactory : AbstractTilemapFactory
{

    protected override string GenerateTilemapName()
    {
        return "Walls";
    }

    protected override void PopulateTilemap(LevelConfig levelConfig, LevelTiles spriteSet)
    {
        foreach(var colliderPosition in levelConfig.Collision.Select(p => new Vector3Int((int)p.x, (int)p.y, 0)))
        {
            tilemap.SetTile(colliderPosition, spriteSet.Wall);
        }
    }
}
