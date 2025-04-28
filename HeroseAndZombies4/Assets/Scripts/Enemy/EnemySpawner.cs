using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public int count;
        public float delayBetweenSpawns = 1f;
    }

    [SerializeField] private Wave[] waves;
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private Transform[] spawnPoints;

    private int currentWaveIndex = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while(currentWaveIndex < waves.Length)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            
            Wave currentWave = waves[currentWaveIndex];
            for(int i = 0; i < currentWave.count; i++)
            {
                SpawnEnemy(currentWave.enemyPrefab);
                yield return new WaitForSeconds(currentWave.delayBetweenSpawns);
            }
            
            currentWaveIndex++;
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}