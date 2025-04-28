using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Vector3 offset;
    
    private Camera cam;
    private Enemy enemy;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void Initialize(Enemy targetEnemy, int maxHealth)
    {
        enemy = targetEnemy;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        
        enemy.OnTakeDamage.AddListener(HandleDamageTaken);
        enemy.OnDeath.AddListener(OnEnemyDeath);
    }

    public void HandleDamageTaken()
    {
        slider.value = enemy.CurrentHealth;
    }

    private void OnEnemyDeath()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if(enemy != null && cam != null)
            transform.position = cam.WorldToScreenPoint(enemy.transform.position + offset);
    }

    private void OnDestroy()
    {
        if (enemy != null)
        {
            enemy.OnTakeDamage.RemoveListener(HandleDamageTaken);
            enemy.OnDeath.RemoveListener(OnEnemyDeath);
        }
    }
}