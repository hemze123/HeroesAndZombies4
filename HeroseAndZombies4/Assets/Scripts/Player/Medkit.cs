using UnityEngine;
using System.Collections;

public class Medkit : MonoBehaviour, ICollectible
{
    [Header("Settings")]
    [SerializeField] private float healAmount = 25f;
    [SerializeField] private float respawnTime = 30f;
    
    [Header("Effects")]
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private ParticleSystem healEffect;

    private Collider col;
    private MeshRenderer[] renderers;

    void Awake()
    {
        col = GetComponent<Collider>();
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void Collect()
    {
        if (Collector.Instance != null)
        {
            Collector.Instance.Heal(healAmount);
            PlayEffects();
            StartCoroutine(Respawn());
        }
    }

    void PlayEffects()
    {
        if (collectSound != null)
            AudioManager.Instance.PlayAtPosition(collectSound.name, transform.position);
        
        if (healEffect != null)
            Instantiate(healEffect, transform.position, Quaternion.identity);
    }

    IEnumerator Respawn()
    {
        SetActive(false);
        yield return new WaitForSeconds(respawnTime);
        SetActive(true);
    }

    void SetActive(bool state)
    {
        col.enabled = state;
        foreach (var renderer in renderers)
        {
            renderer.enabled = state;
        }
    }
}