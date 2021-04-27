using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUiController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerLabel = null;
    [SerializeField] private Button resetButton = null;
    [SerializeField] private Button startButton = null;

    private LevelScoreData currentLevelScore;
    private GameManager gameManager;


    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        RegisterClickListeners();
    }

    void Update()
    {
        timerLabel.SetText(new TimeSpan((long)(currentLevelScore?.CompletionTime * 10000 ?? 0)).ToString(@"mm\:ss"));
    }

    private void RegisterClickListeners()
    {
        resetButton?.onClick.AddListener(OnResetLevelButtonClick);
        startButton?.onClick.AddListener(OnStartLevelButtonClick);
    }

    private void OnResetLevelButtonClick()
    {
        currentLevelScore = gameManager.ResetLevel();
    }

    private void OnStartLevelButtonClick()
    {
        currentLevelScore = gameManager.StartLevel();
        startButton.gameObject.SetActive(false);
    }
}
