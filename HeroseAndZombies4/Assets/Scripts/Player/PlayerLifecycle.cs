using UnityEngine;

public class PlayerLifecycle : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private float respawnDelay = 2f;

    void OnEnable() => Collector.Instance.OnDeath.AddListener(OnDeath);
    void OnDisable() => Collector.Instance.OnDeath.RemoveListener(OnDeath);

    void OnDeath()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        
        Invoke(nameof(Respawn), respawnDelay);
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
        gameObject.SetActive(true);
        Collector.Instance.ResetHealth();
        
        GameEvents.OnPlayerRespawn?.Invoke();
    }
}