public abstract class AbstractTilemapFactory : TilemapWriter
{
    public void WriteTiles(LevelConfig levelData, LevelTiles spriteSet)
    {
        PopulateTilemap(levelData, spriteSet);
    }

    public override void CleanTilemap()
    {
        tilemap.ClearAllTiles(); 
    }

    protected abstract void PopulateTilemap( LevelConfig levelConfig, LevelTiles spriteSet);
}