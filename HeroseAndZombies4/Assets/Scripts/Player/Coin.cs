using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    [Header("Settings")]
    [SerializeField] private int value = 1;
    [SerializeField] private float collectRadius = 0.5f;
    
    [Header("Effects")]
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private ParticleSystem collectEffect;
    
    private bool isCollected;

    public void Collect()
    {
        if (isCollected) return;
        
        isCollected = true;
        EconomyManager.Instance.AddCoins(value);
        
        PlayEffects();
        ReturnToPool();
    }

    void PlayEffects()
    {
        if (collectSound != null)
            AudioManager.Instance.PlayAtPosition(collectSound.name, transform.position);
        
        if (collectEffect != null)
            VFXManager.Instance.PlayEffect(collectEffect.name, transform.position);
    }

    void ReturnToPool()
    {
        ObjectPooler.Instance.ReturnToPool(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
}