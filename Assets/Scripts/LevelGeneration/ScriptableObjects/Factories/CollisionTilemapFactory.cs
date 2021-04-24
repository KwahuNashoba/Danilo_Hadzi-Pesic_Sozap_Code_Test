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
        foreach(var colliderPosition in levelConfig.Collision)
        {
            tilemap.SetTile((Vector3Int)colliderPosition, spriteSet.Wall);
        }
    }
}
