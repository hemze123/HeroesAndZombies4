using UnityEngine;

public class LODManager : MonoBehaviour
{
    public static LODManager Instance { get; private set; }

    [System.Serializable]
    public class LODSetting
    {
        public float distanceThreshold;
        public int lodLevel;
        public bool disableShadows;
    }

    [SerializeField] private LODSetting[] lodSettings;
    [SerializeField] private float updateInterval = 1f;
    
    private Transform player;
    private float lastUpdateTime;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Time.time - lastUpdateTime > updateInterval)
        {
            UpdateAllLODs();
            lastUpdateTime = Time.time;
        }
    }

    void UpdateAllLODs()
    {
        LODGroup[] allLODGroups = FindObjectsOfType<LODGroup>();
        
        foreach (LODGroup lodGroup in allLODGroups)
        {
            float distance = Vector3.Distance(player.position, lodGroup.transform.position);
            UpdateLOD(lodGroup, distance);
        }
    }

    void UpdateLOD(LODGroup lodGroup, float distance)
    {
        for (int i = 0; i < lodSettings.Length; i++)
        {
            if (distance <= lodSettings[i].distanceThreshold)
            {
                lodGroup.ForceLOD(lodSettings[i].lodLevel);
                
                // Shadow optimizasyonu
                Renderer[] renderers = lodGroup.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    r.shadowCastingMode = lodSettings[i].disableShadows ? 
                        UnityEngine.Rendering.ShadowCastingMode.Off : 
                        UnityEngine.Rendering.ShadowCastingMode.On;
                }
                
                break;
            }
        }
    }
}