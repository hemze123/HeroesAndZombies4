using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text healthText;

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        
        if (healthText != null)
            healthText.text = $"{Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(maxHealth)}";
    }
}