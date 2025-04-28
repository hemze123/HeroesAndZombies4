using System;
using UnityEngine;

public class DailyRewards : MonoBehaviour
{
    public static DailyRewards Instance { get; private set; }

    [SerializeField] private int[] rewardAmounts = { 50, 100, 150, 200, 300 };
    
    private const string LAST_CLAIM_KEY = "last_reward_claim";
    private const string STREAK_KEY = "reward_streak";

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool CanClaimReward()
    {
        DateTime lastClaim = GetLastClaimDate();
        return DateTime.Today > lastClaim.Date;
    }

    public int ClaimReward()
    {
        DateTime lastClaim = GetLastClaimDate();
        int streak = GetCurrentStreak();

        // Streak sıfırla veya arttır
        streak = (DateTime.Today - lastClaim.Date).Days == 1 ? streak + 1 : 1;

        // Ödül miktarını belirle
        int rewardIndex = Mathf.Clamp(streak - 1, 0, rewardAmounts.Length - 1);
        int reward = rewardAmounts[rewardIndex];

        // Kaydet
        PlayerPrefs.SetString(LAST_CLAIM_KEY, DateTime.Today.ToString());
        PlayerPrefs.SetInt(STREAK_KEY, streak);
        EconomyManager.Instance.AddCoins(reward);

        return reward;
    }

    public int GetCurrentStreak()
    {
        DateTime lastClaim = GetLastClaimDate();
        int streak = PlayerPrefs.GetInt(STREAK_KEY, 0);
        
        // Eğer 1 günden fazla geçmişse streak'i sıfırla
        if ((DateTime.Today - lastClaim.Date).Days > 1)
        {
            streak = 0;
            PlayerPrefs.SetInt(STREAK_KEY, 0);
        }

        return streak;
    }

    public int GetNextRewardAmount()
    {
        int nextStreak = GetCurrentStreak() + 1;
        int rewardIndex = Mathf.Clamp(nextStreak - 1, 0, rewardAmounts.Length - 1);
        return rewardAmounts[rewardIndex];
    }

    private DateTime GetLastClaimDate()
    {
        string dateString = PlayerPrefs.GetString(LAST_CLAIM_KEY, DateTime.MinValue.ToString());
        return DateTime.Parse(dateString);
    }
}