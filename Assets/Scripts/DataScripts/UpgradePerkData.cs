using UnityEngine;

public enum StatType
{
    Damage,
    FireRate,
    RotationSpeed,
    projectileSpeed
}

[CreateAssetMenu(fileName = "New Upgrade Data", menuName = "Game Data/Perks/Upgrade Perk")]
public class UpgradePerkData : PerkData
{
    [Header("강화 능력")]
    public StatType statToUpgrade;
    public float valuePerLevel;
    public bool isPercentage;
}
