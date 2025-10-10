using UnityEngine;

public class TowerStatManager : MonoBehaviour
{
    public static TowerStatManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public float TotalDamageMultiplier { get; private set; } = 1.0f;
    public float TotalFireRateMultiplier { get; private set; } = 1.0f;
    public float TotalRotationSpeedMultiplier { get; private set; } = 1.0f;
    public float TotalProjectileSpeedMultiplier { get; private set; } = 1.0f;
    private PerkInventory _playerInventory;

    public void RegisterPlayerInventory(PerkInventory inventory)
    {
        _playerInventory = inventory;
    }

    public void RecalculateStats()
    {
        if (_playerInventory == null) return;

        TotalDamageMultiplier = 1.0f;
        TotalFireRateMultiplier = 1.0f;
        TotalRotationSpeedMultiplier = 1.0f;

        foreach (PerkStatus status in _playerInventory.GetAllOwnedPerks())
        {
            if (status.perkData is UpgradePerkData upgradePerkData)
            {
                float bounsValue = upgradePerkData.valuePerLevel * status.currentLevel;

                switch (upgradePerkData.statToUpgrade)
                {
                    case StatType.Damage:
                        TotalDamageMultiplier += bounsValue;
                        break;
                    case StatType.FireRate:
                        TotalFireRateMultiplier += bounsValue;
                        break;
                    case StatType.RotationSpeed:
                        TotalRotationSpeedMultiplier += bounsValue;
                        break;
                    case StatType.projectileSpeed:
                        TotalProjectileSpeedMultiplier += bounsValue;
                        break;
                }
            }
        }
        Debug.Log($"스탯 재계산... 데미지 x {TotalDamageMultiplier}, 공속 x {TotalFireRateMultiplier}");
    }
}
