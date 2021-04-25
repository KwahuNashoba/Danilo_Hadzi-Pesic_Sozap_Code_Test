using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGenerator = null;
    [SerializeField] private Sprite boxImage = null;
    [SerializeField] private GameObject playerPrefab = null;

    private GameController gameController;

    void Start()
    {
        var demoLevelConfig = new LevelConfig()
        {
            Collision = new Vector2Int[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(0, 3),
                new Vector2Int(1, 3),
                new Vector2Int(2, 3),
                new Vector2Int(3, 3),
                new Vector2Int(4, 3),
                new Vector2Int(4, 2),
                new Vector2Int(4, 1),
                new Vector2Int(4, 0),
                new Vector2Int(3, 0),
                new Vector2Int(2, 0),
                new Vector2Int(1, 0),
            },
            Walkable = new Vector2Int[]
            {
                new Vector2Int(1, 1),
                new Vector2Int(1, 2),
                new Vector2Int(2, 1),
                new Vector2Int(2, 2),
                new Vector2Int(3, 1),
                new Vector2Int(3, 2),
            },
            BoxHolders = new Vector2Int[]
            {
                new Vector2Int(2,2)
            },
            Boxes = new Vector2Int[]
            {
                new Vector2Int(2,1)
            },
            StartPosition = new Vector2Int(1, 1)
        };

        InitializeLevel(demoLevelConfig);
    }

    private void InitializeLevel(LevelConfig levelConfig)
    { 
        // initialize level layout
        levelGenerator.GenerateLevel(levelConfig);

        // initialize boxes
        // TODO: this could have been prefab instead of building it from code
        var emptyBoxObject = new GameObject("Box");
        var boxes = levelConfig.Boxes.Select(boxPosition =>
        {
            var box = GameObject.Instantiate(
                emptyBoxObject,
                new Vector3(boxPosition.x, boxPosition.y, 0f),
                Quaternion.identity,
                levelGenerator.transform)
            .AddComponent<SpriteRenderer>();
            box.sprite = boxImage;
            return new KeyValuePair<Vector2Int, Transform>(
                new Vector2Int((int)box.transform.position.x, (int)box.transform.position.y),
                box.transform);
        }).ToDictionary(x => x.Key,x => x.Value);
        Destroy(emptyBoxObject);

        // initialize player
        var player = GameObject.Instantiate(
            playerPrefab,
            new Vector3(levelConfig.StartPosition.x, levelConfig.StartPosition.y, 0f),
            Quaternion.identity,
            levelGenerator.transform)
            .GetComponent<PlayerController>();
            

        gameController = new GameController(levelConfig, boxes, player);
    }
}
