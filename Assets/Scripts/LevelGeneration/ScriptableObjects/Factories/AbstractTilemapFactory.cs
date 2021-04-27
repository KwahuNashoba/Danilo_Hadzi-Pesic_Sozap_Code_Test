public abstract class AbstractTilemapFactory : TilemapWriter
{
    public void WriteTiles(LevelConfigData levelData, LevelTiles spriteSet)
    {
        PopulateTilemap(levelData, spriteSet);
    }

    public override void CleanTilemap()
    {
        tilemap.ClearAllTiles(); 
    }

    protected abstract void PopulateTilemap( LevelConfigData levelConfig, LevelTiles spriteSet);
}