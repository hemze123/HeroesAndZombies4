using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [System.Serializable]
    public class VFX
    {
        public string name;
        public ParticleSystem prefab;
        public int poolSize = 5;
        [HideInInspector] public Queue<ParticleSystem> pool = new Queue<ParticleSystem>();
    }

    [SerializeField] private List<VFX> effects = new List<VFX>();
    private Dictionary<string, VFX> vfxDictionary = new Dictionary<string, VFX>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (VFX effect in effects)
        {
            vfxDictionary.Add(effect.name, effect);
            WarmPool(effect);
        }
    }

    void WarmPool(VFX effect)
    {
        for (int i = 0; i < effect.poolSize; i++)
        {
            ParticleSystem vfx = Instantiate(effect.prefab, transform);
            vfx.gameObject.SetActive(false);
            effect.pool.Enqueue(vfx);
        }
    }

    public void PlayEffect(string effectName, Vector3 position, Quaternion rotation = default)
    {
        if (vfxDictionary.TryGetValue(effectName, out VFX effect))
        {
            ParticleSystem vfx = GetVFXFromPool(effect);
            vfx.transform.position = position;
            vfx.transform.rotation = rotation;
            vfx.gameObject.SetActive(true);
            vfx.Play();

            StartCoroutine(ReturnToPool(vfx, effect));
        }
        else
        {
            Debug.LogWarning($"VFX '{effectName}' not found!");
        }
    }

    ParticleSystem GetVFXFromPool(VFX effect)
    {
        if (effect.pool.Count == 0)
        {
            // Pool büyütme
            ParticleSystem newVFX = Instantiate(effect.prefab, transform);
            effect.pool.Enqueue(newVFX);
            Debug.Log($"Expanding pool for: {effect.name}");
        }

        return effect.pool.Dequeue();
    }

     private IEnumerator ReturnToPool(ParticleSystem vfx, VFX effect)
    {
        yield return new WaitWhile(() => vfx.isPlaying);
        vfx.gameObject.SetActive(false);
        effect.pool.Enqueue(vfx);
    }

    public void StopAllEffects(string effectName)
    {
        if (vfxDictionary.TryGetValue(effectName, out VFX effect))
        {
            foreach (ParticleSystem vfx in effect.pool)
            {
                if (vfx.gameObject.activeInHierarchy)
                {
                    vfx.Stop();
                    vfx.gameObject.SetActive(false);
                }
            }
        }
    }
}