using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGenerator = null;
    [SerializeField] private Sprite boxImage = null;
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private GameSceneUiController uiController = null;

    private LevelConfigData currentLevelConfig;
    private Dictionary<string, LevelConfigData> levelConfigs;

    private PlayerInput inputControlls;
    private GameplayController gameplayController;
    private Coroutine timerCoroutine;

    private Dictionary<Vector2, Transform> boxes;
    private PlayerController playerController;

    private LevelScoreData currentLevelScore;
    private Dictionary<string, LevelScoreData> bestScores;

    void Start()
    {
        LoadLevelConfigs();
        LoadBestScores();
        currentLevelConfig = levelConfigs["1"];
        inputControlls = new PlayerInput();

        InitializeUi();
    }

    public LevelScoreData StartLevel()
    {
        InitializeLevel(currentLevelConfig);
        ResetTimer();

        return currentLevelScore;
    }

    public LevelScoreData ResetLevel()
    {
        InitializeLevel(currentLevelConfig);
        ResetTimer();

        return currentLevelScore;
    }

    private void InitializeLevel(LevelConfigData levelConfig)
    { 
        currentLevelScore = new LevelScoreData();
        
        levelGenerator.GenerateLevel(levelConfig);
        InitializeBoxes(levelConfig);
        InitializePlayer(levelConfig);
        InitializeGamePlayController(levelConfig);
    }

    private void InitializeUi()
    {
        uiController.Init(this);
    }

    private void InitializeBoxes(LevelConfigData levelConfig)
    {
        // clean previous state
        if(boxes != null)
        {
            foreach(var box in boxes)
            {
                Destroy(box.Value.gameObject);
            }
            boxes.Clear();
        }

        // TODO: this could have been prefab instead of building it from code
        var emptyBoxObject = new GameObject("Box");
        boxes = levelConfig.Boxes.Select(boxPosition =>
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
        }).ToDictionary(x => x.Key, x => x.Value);

        Destroy(emptyBoxObject);
    }

    private void InitializePlayer(LevelConfigData levelConfig)
    {
        if(playerController != null)
        {
            DestroyImmediate(playerController.gameObject);
        }

        playerController = GameObject.Instantiate(
            playerPrefab,
            new Vector3(levelConfig.StartPosition.x, levelConfig.StartPosition.y, 0f),
            Quaternion.identity,
            levelGenerator.transform)
            .GetComponent<PlayerController>();
    }

    private void InitializeGamePlayController(LevelConfigData levelConfig)
    {
        gameplayController =
            new GameplayController(levelConfig, boxes, playerController, currentLevelScore, inputControlls);
        gameplayController.LevelCompleted?.AddListener(OnLevelCompleted);
    }

    private void ResetTimer()
    {
        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(LevelTimer());
    }

    private IEnumerator LevelTimer()
    {
        currentLevelScore.CompletionTime = 0;
        while (true)
        {
            currentLevelScore.CompletionTime += (long)(Time.deltaTime * 1000);
            yield return null;
        }
    }

    private void OnLevelCompleted()
    {
        StopCoroutine(timerCoroutine);
        RegisterCurrentScore();
    }

    private void RegisterCurrentScore()
    {
        string levelId = currentLevelConfig.LevelId;

        if(bestScores.TryGetValue(levelId, out LevelScoreData scoreData))
        {
            bestScores[levelId] = currentLevelScore.CompletionTime > scoreData.CompletionTime
                ? currentLevelScore : scoreData;
        }
        else
        {
            bestScores[levelId] = currentLevelScore;
        }

        bestScores[levelId].TotalAttempts++;
        SaveBestScores();
    }

    private void LoadLevelConfigs()
    {
        levelConfigs = Resources.LoadAll<TextAsset>("LevelConfigs").ToDictionary(
            c => c.name, c => JsonUtility.FromJson<LevelConfigData>(c.text));
    }

    private void LoadBestScores()
    {
        bestScores = SimpleSaveSystem.LoadFromDrive<Dictionary<string, LevelScoreData>>(
                typeof(LevelScoreData).Name)
            ?? new Dictionary<string, LevelScoreData>();
    }

    private void SaveBestScores()
    {
        SimpleSaveSystem.SaveObjectToDisk(typeof(LevelScoreData).Name, bestScores);
    }
}
