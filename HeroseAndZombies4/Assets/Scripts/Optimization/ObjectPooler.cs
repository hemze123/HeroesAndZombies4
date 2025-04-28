using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
    }

    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePools();
    }

    void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(pool.parent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
            ExpandPool(tag, 5); // Otomatik genişletme

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        // Havuzdan çıkarılan objeleri dinleme
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnSpawn();
        }

        return objectToSpawn;
    }

    public void ReturnToPool(GameObject obj)
    {
        string tag = obj.name.Replace("(Clone)", "").Trim();
        
        if (poolDictionary.ContainsKey(tag))
        {
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning($"Trying to return non-pooled object: {obj.name}");
            Destroy(obj);
        }
    }

    void ExpandPool(string tag, int amount)
    {
        Pool pool = pools.Find(p => p.tag == tag);
        if (pool == null) return;

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.transform.SetParent(pool.parent);
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }

        Debug.Log($"Expanded pool {tag} by {amount} objects");
    }
}

public interface IPooledObject
{
    void OnSpawn(); // Obje havuzdan çıkarıldığında çağrılır
}