using UnityEngine;
using UnityEngine.Events;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [SerializeField] private int startingCoins = 500;
    private int currentCoins;

    public UnityEvent<int> OnCoinsChanged;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadCoins();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        SaveCoins();
        OnCoinsChanged?.Invoke(currentCoins);
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            SaveCoins();
            OnCoinsChanged?.Invoke(currentCoins);
            return true;
        }
        return false;
    }

    public int GetCurrentCoins() => currentCoins;

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("player_coins", currentCoins);
    }

    private void LoadCoins()
    {
        currentCoins = PlayerPrefs.GetInt("player_coins", startingCoins);
    }

    public void ResetEconomy()
    {
        currentCoins = startingCoins;
        SaveCoins();
        OnCoinsChanged?.Invoke(currentCoins);
    }
}
