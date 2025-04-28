using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public enum GameState { Menu, Playing, Paused, GameOver, Loading }
    public GameState CurrentState { get; private set; }
    
    [SerializeField] private int targetFrameRate = 60;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        Application.targetFrameRate = targetFrameRate;
        CurrentState = GameState.Menu;
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        GameEvents.OnGameStateChanged?.Invoke(newState);
        
        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0.5f; // Yavaş çekim efekti
                break;
        }
    }

    public void QuitGame()
    {
        SaveManager.Instance.SaveAll();
        Application.Quit();
    }
}