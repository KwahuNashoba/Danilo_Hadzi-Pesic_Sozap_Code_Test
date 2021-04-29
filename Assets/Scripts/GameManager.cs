using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    [SerializeField] private LevelGenerator levelGenerator = null;
    [SerializeField] private Sprite boxImage = null;
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private GameSceneUiController uiController = null;

    public Dictionary<string, LevelScoreData> BestScores { get; private set; }
    public Dictionary<string, LevelConfigData> LevelConfigs { get; private set; }

    public UnityEvent<string> LevelCompleted = new UnityEvent<string>();
    
    private LevelConfigData currentLevelConfig;

    private PlayerInput inputControlls;
    private GameplayController gameplayController;
    private Coroutine timerCoroutine;

    private Dictionary<Vector2, Transform> boxes;
    private PlayerController playerController;

    private LevelScoreData currentLevelScore;

    void Start()
    {
        LoadLevelConfigs();
        LoadBestScores();
        inputControlls = new PlayerInput();

        InitializeUi();
    }

    public LevelScoreData StartLevel(string levelId)
    {
        if(LevelConfigs.TryGetValue(levelId, out LevelConfigData config))
        {
            currentLevelConfig = config;
            InitializeLevel(currentLevelConfig);
            ResetTimer();
            RegisterLevelAttampt(currentLevelConfig);
            return currentLevelScore;
        }
        else
        {
            // TODO: error handling
            return null;
        }

    }

    private void InitializeLevel(LevelConfigData levelConfig)
    {
        currentLevelScore = new LevelScoreData() { LevelId = levelConfig.LevelId };
        
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

        LevelCompleted?.Invoke(currentLevelConfig.LevelId);
    }

    private void RegisterCurrentScore()
    {
        string levelId = currentLevelConfig.LevelId;

        if(BestScores.TryGetValue(levelId, out LevelScoreData scoreData))
        {
            scoreData.Completed = true;
            scoreData.CompletionTime = currentLevelScore.CompletionTime < scoreData.CompletionTime
                ? currentLevelScore.CompletionTime : scoreData.CompletionTime;
        }
        else
        {
            currentLevelScore.Completed = true;
            BestScores[levelId] = currentLevelScore;
        }

        SaveBestScores();
    }

    private void RegisterLevelAttampt(LevelConfigData levelConfig)
    {
        if(BestScores.TryGetValue(levelConfig.LevelId, out LevelScoreData score))
        {
            score.TotalAttempts++;
        }
        else
        {
            BestScores[levelConfig.LevelId] = new LevelScoreData()
            {
                LevelId = levelConfig.LevelId,
                CompletionTime = float.MaxValue,
                TotalAttempts = 1,
                Completed = false
            };
        }

        SaveBestScores();
    }

    private void LoadLevelConfigs()
    {
        LevelConfigs = Resources.LoadAll<TextAsset>("LevelConfigs").ToDictionary(
            c => c.name, c => JsonUtility.FromJson<LevelConfigData>(c.text));
    }

    private void LoadBestScores()
    {
        string SavesPath = $"{Application.persistentDataPath}/{typeof(LevelScoreData).Name}.ssave";
        BestScores = SimpleSaveSystem.LoadFromDrive<Dictionary<string, LevelScoreData>>(SavesPath)
            ?? new Dictionary<string, LevelScoreData>();
    }

    private void SaveBestScores()
    {
        string SavesPath = $"{Application.persistentDataPath}/{typeof(LevelScoreData).Name}.ssave";
        SimpleSaveSystem.SaveObjectToDisk(SavesPath, BestScores);
    }
}
