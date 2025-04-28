using UnityEngine;
public static class PriceCalculator
{
    public static int CalculateItemPrice(int basePrice, int priceIncrease, int timesPurchased)
    {
        return basePrice + (priceIncrease * timesPurchased);
    }

    public static int CalculateSellPrice(int purchasePrice)
    {
        return Mathf.RoundToInt(purchasePrice * 0.7f); // %70 iade
    }

    public static int CalculateUpgradeCost(int baseCost, int upgradeLevel)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(1.2f, upgradeLevel));
    }
}