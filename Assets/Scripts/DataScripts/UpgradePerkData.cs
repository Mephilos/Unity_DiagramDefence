using UnityEngine;

public enum StatType
{
    Damage,
    FireRate,
    RotationSpeed,
    Pierce
}

[CreateAssetMenu(fileName = "New Upgrade Data", menuName = "Game Data/Perks/Upgrade Perk")]
public class UpgradePerkData : PerkData
{
    [Header("강화 능력")]
    public StatType statToUpgrade;
    public float valuePerLevel; // 퍽 레벨 증가시 증가하는 비율
    public bool isPercentage;
}
