using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TilemapWriter : ScriptableObject
{
    protected Tilemap tilemap;

    // used to initialize state of loaded scriptable objects because
    // they don't get to call constructors
    public void Init(Transform parentGrid, GameObject tilemapTemplate)
    {
        tilemap = CreateTilemap(parentGrid, tilemapTemplate);
    }

    public Tilemap CreateTilemap(Transform parentGrid, GameObject tilemapTemplate)
    {
        GameObject tilemapObject = Instantiate(tilemapTemplate, parentGrid);
        tilemapObject.name = GenerateTilemapName();
        return tilemapObject.GetComponent<Tilemap>();
    }

    protected abstract string GenerateTilemapName();
    public abstract void CleanTilemap();
}