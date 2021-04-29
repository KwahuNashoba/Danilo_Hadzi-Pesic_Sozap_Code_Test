using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseLevelPopupUiController : MonoBehaviour
{
    [SerializeField] private Dropdown levelsDropdown = null;
    [SerializeField] private Text bestTimeText = null;
    [SerializeField] private Text totalAttemptsText = null;
    [SerializeField] private PopupAnimator popupAnimator = null;
    [SerializeField] private Button playButton = null;

    public UnityEvent<string> PlayClicked = new UnityEvent<string>();

    private GameManager gameManager;

    void Start()
    {
        RegisterClickListeners();
    }
    
    public void PopulateLevels(GameManager manager)
    {
        gameManager = manager;

        levelsDropdown.ClearOptions();

        // TODO: I definitley need level/score manager

        var availableLevels = gameManager.BestScores
            .Where(score => score.Value.Completed).ToList();
        if(availableLevels.Any())
        {
            levelsDropdown.AddOptions(availableLevels
                .Select(c => $"Level {c.Value.LevelId}").ToList());
            var lastCompletedLevelId = int.Parse(availableLevels.Last().Value.LevelId);
            if (gameManager.LevelConfigs.Keys.Where(key => int.Parse(key) >= lastCompletedLevelId + 1).Any())
            {
                levelsDropdown.AddOptions(new List<string>(){ $"Level {lastCompletedLevelId + 1}"});
            }
        }
        else
        {
            levelsDropdown.AddOptions(new List<string> { "Level 1" });
        }

        levelsDropdown.onValueChanged.AddListener(OnLevelSelected);

        // select next unfinished level
        int optionsIndex = FindHighestLevelCompleted();
        optionsIndex = optionsIndex < levelsDropdown.options.Count ? optionsIndex : levelsDropdown.options.Count - 1;
        levelsDropdown.SetValueWithoutNotify(optionsIndex);
        OnLevelSelected(optionsIndex); // <- yeah, dirty

        popupAnimator.Animate(up: true);
    }

    private void OnLevelSelected(int selectedIndex)
    {
        // ugly, but let's keep it for prototype
        string levelId = levelsDropdown.options[selectedIndex].text.Substring(6);

        string bestTime = "N/A";
        string totalAttempts = "N/A";
        if(gameManager.BestScores.TryGetValue(levelId, out LevelScoreData score))
        {
            if(score.Completed)
            {
                bestTime = new TimeSpan((long)(score.CompletionTime * 10000)).ToString(@"mm\:ss");
            }
            totalAttempts = score.TotalAttempts.ToString();
        }

        bestTimeText.text = $"Best Time: {bestTime}";
        totalAttemptsText.text = $"Total Attempts: {totalAttempts}";
    }

    private void RegisterClickListeners()
    {
        playButton?.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        string levelId = levelsDropdown.options[levelsDropdown.value].text.Substring(6);
        popupAnimator.Animate(up: false);

        PlayClicked?.Invoke(levelId);
    }

    private int FindHighestLevelCompleted()
    {
        if(gameManager.BestScores?.Any() ?? false)
        {
            return gameManager.BestScores.Values.Max(s => int.Parse(s.LevelId));
        }
        else
        {
            return 0;
        }
    }
}
