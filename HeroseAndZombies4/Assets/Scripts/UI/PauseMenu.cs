using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    void ResumeGame()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
    }

    void OpenSettings()
    {
        // Ayarlar menüsünü aç
        Debug.Log("Settings opened");
    }

    void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("MainMenu");
    }
}