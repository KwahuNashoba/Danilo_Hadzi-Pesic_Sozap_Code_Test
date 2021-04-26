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
            Collision = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 2),
                new Vector2(0, 3),
                new Vector2(1, 3),
                new Vector2(2, 3),
                new Vector2(3, 3),
                new Vector2(4, 3),
                new Vector2(5, 3),
                new Vector2(5, 2),
                new Vector2(5, 1),
                new Vector2(5, 0),
                new Vector2(4, 0),
                new Vector2(3, 0),
                new Vector2(2, 0),
                new Vector2(1, 0),
            },

            Walkable = new Vector2[]
            {
                new Vector2(1, 1),
                new Vector2(1, 2),
                new Vector2(2, 1),
                new Vector2(2, 2),
                new Vector2(3, 1),
                new Vector2(3, 2),
                new Vector2(4, 1),
                new Vector2(4, 2),
            },

            BoxHolders = new Vector2[]
            {
                new Vector2(4,2),
                new Vector2(4,1)
            },

            Boxes = new Vector2[]
            {
                new Vector2(3,2),
                new Vector2(3,1)
            },

            StartPosition = new Vector2(1, 1)
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
            return new KeyValuePair<Vector2, Transform>(
                new Vector2(boxPosition.x, boxPosition.y),
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
