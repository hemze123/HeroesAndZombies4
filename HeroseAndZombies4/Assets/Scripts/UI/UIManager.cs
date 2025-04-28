using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Player UI")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GunBar gunBar;
    [SerializeField] private TMP_Text coinText; // Eklendi
    [SerializeField] private TMP_Text scoreText; // Eklendi
    [SerializeField] private TMP_Text interactPrompt;

     [Header("Weapon UI")]
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Image weaponIcon;


    [Header("Menus")]
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameOverMenu gameOverMenu;
    [SerializeField] private GameObject mobileControls;



    private int currentScore; // Skor takibi için

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        #if !UNITY_ANDROID && !UNITY_IOS
        mobileControls.SetActive(false);
        #endif
    }

    // COIN GÜNCELLEME
    public void UpdateCoinUI(int coins)
    {
        coinText.text = $"Coins: {coins}";
    }

    // SCORE GÜNCELLEME
    public void UpdateScoreUI(int newPoints)
    {
        currentScore += newPoints;
        scoreText.text = $"Score: {currentScore}";
    }


     // Silah UI güncelleme metodu eklendi
    public void UpdateWeaponUI(Weapon weapon)
    {
        if (weapon == null) return;
        
        ammoText.text = $"{weapon.stats.currentAmmo}/{weapon.stats.maxAmmo}";
        weaponIcon.sprite = weapon.weaponIcon; // Weapon sınıfına icon eklemeyi unutmayın
    }

    // DİĞER METOTLAR AYNEN KALIYOR...
    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        healthBar.UpdateHealth(currentHealth, maxHealth);
    }

    public void ShowGameOver(int score, int kills)
    {
        gameOverMenu.ShowResults(currentScore, kills); // currentScore kullanılıyor
    }
}