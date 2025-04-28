using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private float invincibilityTime = 1.5f;
    [SerializeField] private LayerMask damageLayers;
    [SerializeField] private LayerMask coinLayers; // Yeni eklenen coin layer'ı
    [SerializeField] private GameObject hitEffect;

    private float lastHitTime;

    // Çarpışma algılama (DÜZELTİLMİŞ HALİ)
    void OnTriggerEnter(Collider other)
    {
        // Hasar kontrolü
        if((damageLayers & (1 << other.gameObject.layer)) != 0)
        {
            TakeDamage(10, other.transform.position);
        }
        // Coin kontrolü (YENİ EKLENEN KISIM)
        else if((coinLayers & (1 << other.gameObject.layer)) != 0)
        {
            ICollectible collectible = other.gameObject.GetComponent<ICollectible>();
            if(collectible != null)
            {
                collectible.Collect();
            }
        }
    }

    void TakeDamage(int amount, Vector3 hitPosition)
    {
        if(Time.time - lastHitTime < invincibilityTime) return;
        
        lastHitTime = Time.time;
        Collector.Instance.TakeDamage(amount);
        
        if(hitEffect != null)
            Instantiate(hitEffect, hitPosition, Quaternion.identity);
            
        GameEvents.OnPlayerDamaged?.Invoke();
    }
}