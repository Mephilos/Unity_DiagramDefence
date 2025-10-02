using UnityEngine;

public enum PerkRarity
{
    Common,
    Rare
}

// 퍽 추상 클래스
public abstract class PerkData : ScriptableObject
{
    [Header("퍽 정보")]
    public string perkName = "New Perk";
    [TextArea]
    public string description = "Perk description";
    public Sprite icon;
    public int maxLevel = 5;

    public PerkRarity rarity = PerkRarity.Common;
}
