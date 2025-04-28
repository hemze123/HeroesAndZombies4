using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
    [System.Serializable]
    public class LevelData
    {
        public string levelID;
        public string displayName;
        public int requiredCoins;
        public bool isLocked = true;
        public int highScore;
    }

    [SerializeField] private List<LevelData> levels;
    private string currentLevelID;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadLevelData();
    }
    public void CompleteCurrentLevel(int score = 1000)
    {
      CompleteLevel(currentLevelID, score);
    } 
    

    public void CompleteLevel(string levelID, int score)
    {
        LevelData level = GetLevel(levelID);
        if (level == null) return;

        level.isLocked = false;
        level.highScore = Mathf.Max(level.highScore, score);
        
        // Sonraki seviyenin kilidini aç
        int nextIndex = levels.FindIndex(l => l.levelID == levelID) + 1;
        if (nextIndex < levels.Count)
        {
            levels[nextIndex].isLocked = false;
        }

        SaveLevelData();
    }

    public void SaveLevelData()
    {
        SaveManager.Instance.SaveObject("level_data", levels);
    }

   private void LoadLevelData()
   {
    if (SaveManager.Instance != null)
    {
        levels = SaveManager.Instance.LoadObject<List<LevelData>>("level_data") ?? levels;
    }
    else
    {
        Debug.LogWarning("SaveManager.Instance bulunamadı, LevelData yüklenemedi!");
    }
  }


    public LevelData GetLevel(string levelID) => levels.Find(l => l.levelID == levelID);
}