using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text highScoreText;

    [Header("Buttons")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(RetryLevel);
        menuButton.onClick.AddListener(ReturnToMenu);
        gameObject.SetActive(false);
    }

    public void ShowResults(int score, int kills)
    {
        scoreText.text = $"Score: {score}";
        killsText.text = $"Kills: {kills}";
        
        int highScore = SaveManager.Instance.LoadInt("high_score", 0);
        if (score > highScore)
        {
            highScore = score;
            SaveManager.Instance.SaveInt("high_score", highScore);
        }
        
        highScoreText.text = $"High Score: {highScore}";
        gameObject.SetActive(true);
    }

    void RetryLevel()
    {
        gameObject.SetActive(false);
        SceneLoader.Instance.ReloadCurrentScene();
    }

    void ReturnToMenu()
    {
        gameObject.SetActive(false);
        SceneLoader.Instance.LoadScene("MainMenu");
    }

    // GameManager.cs kontrolü:
// Metot var mı?
}