using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LevelCreator : MonoBehaviour
{
    private const string LevelConfigsPath = "Assets/Resources/LevelConfigs/";

    [SerializeField] private Button buttonSave = null;
    [SerializeField] private InputField levelIdInput = null;

    [SerializeField] private Tilemap collisionTiles = null;
    [SerializeField] private Tilemap walkableTiles = null;
    [SerializeField] private Tilemap boxHoldersTiles = null;
    [SerializeField] private Tilemap boxesTiles = null;
    [SerializeField] private Tilemap playerTile = null;

    // Start is called before the first frame update
    void Start()
    {
        buttonSave.onClick.AddListener(OnButtonSaveClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonSaveClick()
    {
#if UNITY_EDITOR
        LevelConfigData levelConfig = new LevelConfigData();

        levelConfig.LevelId = levelIdInput.text;
        levelConfig.Collision = TilemapToVector2Positions(collisionTiles);
        levelConfig.Walkable = TilemapToVector2Positions(walkableTiles);
        levelConfig.BoxHolders= TilemapToVector2Positions(boxHoldersTiles);
        levelConfig.Boxes = TilemapToVector2Positions(boxesTiles);
        levelConfig.StartPosition = TilemapToVector2Positions(playerTile).First();
        SimpleSaveSystem.SaveObjectToDisk<string>($"{LevelConfigsPath}{levelIdInput.text}.json", JsonUtility.ToJson(levelConfig));
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private Vector2[] TilemapToVector2Positions(Tilemap tilemap)
    {
        tilemap.CompressBounds();
        var tilePositions = new List<Vector2>();
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if(tilemap.GetTile(pos) != null)
            {
                tilePositions.Add(new Vector2(pos.x, pos.y));
            }
        }

        return tilePositions.ToArray();
    }
}
