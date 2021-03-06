using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Grid))]
public class LevelGenerator : MonoBehaviour
{
    private Grid grid;
    public Grid Grid
    {
        get {
            if (grid == null)
            {
                grid = GetComponent<Grid>();
            }
            return grid;
        }
        private set { grid = value; } 
    }

    [SerializeField] private LevelTiles tileSprites = null;
    [SerializeField] private GameObject tilemapPrefab = null;
    [SerializeField] private List<AbstractTilemapFactory> tileFactories = null;

    private UnityEvent CleanMapEvent;

    public void GenerateLevel(LevelConfigData levelConfig)
    {
        ClearLevel();
        FocusCameraOnLevel(levelConfig);

        foreach(AbstractTilemapFactory f in tileFactories)
        {
            f.Init(transform, tilemapPrefab);
            f.WriteTiles(levelConfig, tileSprites);
            CleanMapEvent.AddListener(f.CleanTilemap);
        }
    }

    private void FocusCameraOnLevel(LevelConfigData levelConfig)
    {
        var collisionList = new List<Vector2>(levelConfig.Collision);
        var levelWidth = collisionList.Max(p => p.x);
        var levelHeight = collisionList.Max(p => p.y);

        Camera.main.transform.position = new Vector3(++levelWidth / 2f, levelHeight / 2, -1);
        Camera.main.orthographicSize = levelWidth + 1;
    }

    private void ClearLevel()
    {
        if(CleanMapEvent == null)
        {
            CleanMapEvent = new UnityEvent();
        }

        CleanMapEvent.Invoke();
        CleanMapEvent.RemoveAllListeners();
    }
}
