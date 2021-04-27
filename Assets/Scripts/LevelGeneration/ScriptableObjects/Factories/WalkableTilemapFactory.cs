using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "WalkableTilesFactory", menuName = "Sozap Test/Walkable Tiles Factory")]
public class WalkableTilemapFactory : AbstractTilemapFactory
{
    private Tilemap secondaryTilemap;

    public override void CleanTilemap()
    {
        base.CleanTilemap();
        secondaryTilemap.ClearAllTiles();
    }

    public override void Init(Transform parentGrid, GameObject tilemapTemplate)
    {
        base.Init(parentGrid, tilemapTemplate);
        if(secondaryTilemap == null)
        {
            secondaryTilemap = CreateTilemap(tilemap.transform, tilemap.gameObject);
        }
    }

    protected override string GenerateTilemapName()
    {
        return "Walkables";
    }

    protected override void PopulateTilemap(LevelConfigData levelConfig, LevelTiles spriteSet)
    {
        PopulatePrimaryMap(levelConfig, spriteSet);
        PopulateSecondaryMap(levelConfig, spriteSet);
    }

    private void PopulatePrimaryMap(LevelConfigData levelConfig, LevelTiles spriteSet)
    {
        foreach(var tilePosition in levelConfig.Walkable.Select(p => new Vector3Int((int)p.x, (int)p.y, 0)))
        {
            tilemap.SetTile(tilePosition, spriteSet.Walkable);
        }
    }

    private void PopulateSecondaryMap(LevelConfigData levelConfig, LevelTiles spriteSet)
    {
        foreach (var tilePosition in levelConfig.BoxHolders.Select(p => new Vector3Int((int)p.x, (int)p.y, 0)))
        {
            secondaryTilemap.SetTile(tilePosition, spriteSet.BoxHolder);
        }
    }
}
