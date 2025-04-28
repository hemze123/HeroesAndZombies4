using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrapper : MonoBehaviour
{
    [Header("Core Prefab")]
    [SerializeField] private GameObject coreGamePrefab;

    [Header("First Scene")]
    [SerializeField] private string firstSceneName = "GameScene"; // İlk oyun sahnenin adı

    private static bool isCoreGameLoaded = false;

    void Awake()
    {
        if (!isCoreGameLoaded)
        {
            Instantiate(coreGamePrefab);
            isCoreGameLoaded = true;
            Debug.Log("CoreGame prefab sahneye yüklendi!");

            // Şimdi asıl oyun sahnesini yükle
            SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Additive);
        }
        else
        {
            Debug.Log("CoreGame zaten yüklenmiş.");
        }
    }
}
