using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "WalkableTilesFactory", menuName = "Sozap Test/Walkable Tiles Factory")]
public class WalkableTilemapFactory : AbstractTilemapFactory
{
    private Tilemap secondaryTilemap;

    protected override string GenerateTilemapName()
    {
        return "Walkables";
    }

    protected override void PopulateTilemap(LevelConfig levelConfig, LevelTiles spriteSet)
    {
        secondaryTilemap = CreateTilemap(tilemap.transform, tilemap.gameObject);
        PopulatePrimaryMap(levelConfig, spriteSet);
        PopulateSecondaryMap(levelConfig, spriteSet);
    }

    private void PopulatePrimaryMap(LevelConfig levelConfig, LevelTiles spriteSet)
    {
        foreach(var tilePosition in levelConfig.Walkable.Select(p => new Vector3Int((int)p.x, (int)p.y, 0)))
        {
            tilemap.SetTile(tilePosition, spriteSet.Walkable);
        }
    }

    private void PopulateSecondaryMap(LevelConfig levelConfig, LevelTiles spriteSet)
    {
        foreach (var tilePosition in levelConfig.BoxHolders.Select(p => new Vector3Int((int)p.x, (int)p.y, 0)))
        {
            secondaryTilemap.SetTile(tilePosition, spriteSet.BoxHolder);
        }
    }
}
