using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUiController : MonoBehaviour
{
    [SerializeField] private Button playButton;

    void Start()
    {
        playButton?.onClick.AddListener(() => SceneManager.LoadSceneAsync(1));
    }
}
