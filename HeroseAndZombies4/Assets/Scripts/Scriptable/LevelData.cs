using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game Data/Level")]
public class LevelData : ScriptableObject
{
    [Header("Basic Info")]
    public string levelName;
    public string sceneName;
    public Sprite previewImage;

    [Header("Progression")]
    public int requiredCoinsToUnlock = 0;
    public LevelData[] nextLevels;

    [Header("Enemy Waves")]
    public Wave[] waves;
    public int totalEnemies;

    [Header("Rewards")]
    public int baseCoinReward = 100;
    public WeaponData[] unlockableWeapons;

    [System.Serializable]
    public class Wave
    {
        public EnemyData enemyType;
        public int count;
        public float spawnInterval = 1f;
        public float delayAfterWave = 5f;
    }
}