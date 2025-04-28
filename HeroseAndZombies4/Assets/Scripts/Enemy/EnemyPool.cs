// EnemySpawner'a ekle:
using UnityEngine;
using System.Collections.Generic;
public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;
    public GameObject enemyPrefab;
    public int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy()
    {
        if(pool.Count == 0) InitializePool();
        return pool.Dequeue();
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }
}