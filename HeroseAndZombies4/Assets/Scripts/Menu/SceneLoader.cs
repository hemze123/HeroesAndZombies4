using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text progressText;
    [SerializeField] private float minLoadTime = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Loading);
        loadingCanvas.SetActive(true);
        
        float timer = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone || timer < minLoadTime)
        {
            timer += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            progressText.text = $"{progress * 100:F0}%";

            if (operation.progress >= 0.9f && timer >= minLoadTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingCanvas.SetActive(false);
        GameEvents.OnSceneLoaded?.Invoke(sceneName);
    }
}