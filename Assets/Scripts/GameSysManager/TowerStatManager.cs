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
    public int AddPierceCount { get; private set; } = 0; // 투사체 관통 횟수
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
        AddPierceCount = 0;

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
                    case StatType.Pierce:
                        AddPierceCount += (int)bounsValue;
                        break;
                }
            }
        }
        Debug.Log($"스탯 재계산... 데미지 x {TotalDamageMultiplier}, 공속 x {TotalFireRateMultiplier}");
    }
}
