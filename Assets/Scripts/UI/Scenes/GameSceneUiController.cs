using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneUiController : MonoBehaviour
{
    [SerializeField] private Text timerLabel = null;
    [SerializeField] private Button resetButton = null;
    [SerializeField] private Button nextLevelButton = null;
    [SerializeField] private Button backButton = null;
    [SerializeField] private GameObject chooseLevelPopupTemplate = null;

    private LevelScoreData currentLevelScore;
    private GameManager gameManager;
    private ChooseLevelPopupUiController chooseLevelPopup;
    private string nextLevelId;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;

        chooseLevelPopup = Instantiate(chooseLevelPopupTemplate, transform.parent)
            .GetComponent<ChooseLevelPopupUiController>();
        chooseLevelPopup.PopulateLevels(gameManager);
        chooseLevelPopup.PlayClicked.AddListener(OnPlayButtonClicked);
        this.gameManager.LevelCompleted.AddListener(OnLevelCompleted);

        RegisterClickListeners();
    }

    void Update()
    {
        // TODO: rework this
        timerLabel.text = new TimeSpan((long)(currentLevelScore?.CompletionTime * 10000 ?? 0)).ToString(@"mm\:ss");
    }

    private void RegisterClickListeners()
    {
        resetButton?.onClick.AddListener(OnResetLevelButtonClick);
        nextLevelButton?.onClick.AddListener(OnNextLevelClicked);
        backButton?.onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    private void OnResetLevelButtonClick()
    {
        currentLevelScore = gameManager.StartLevel(currentLevelScore.LevelId);
    }

    private void OnPlayButtonClicked(string levelId)
    {
        currentLevelScore = gameManager.StartLevel(levelId);
        resetButton.gameObject.SetActive(true);
    }

    private void OnLevelCompleted(string levelId)
    {
        nextLevelId = (int.Parse(levelId) + 1).ToString();
        if (gameManager.LevelConfigs.Keys.Where(key => int.Parse(key) >= int.Parse(nextLevelId)).Any())
        {
            nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            // TODO: new levels comming soon message
        }
    }

    private void OnNextLevelClicked()
    {
        currentLevelScore = gameManager.StartLevel((int.Parse(nextLevelId)).ToString());
        nextLevelButton.gameObject.SetActive(false);
    }
}
