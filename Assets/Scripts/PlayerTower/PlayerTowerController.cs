using System.Collections.Generic;
using UnityEngine;

public class PlayerTowerController : MonoBehaviour
{
    private const int PERK_OPTIONS_COUNT = 3;

    private PlayerStatus _playerStatus;
    private ShapeEquipment _shapeEquipment;
    private PerkInventory _inventory;

    //public TowerData towerData;

    void Awake()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _shapeEquipment = GetComponent<ShapeEquipment>();
        _inventory = GetComponent<PerkInventory>();

        _playerStatus.OnLevelUp += OnPlayerLevelUp;
    }
    
    void OnDestroy()
    {
        _playerStatus.OnLevelUp -= OnPlayerLevelUp;
    }
    
    void Start()
    {
        PerkManager.Instance.RegisterPlayerInventory(_inventory);
        TowerStatManager.Instance.RegisterPlayerInventory(_inventory);
    }

    private void OnPlayerLevelUp()
    {
        Time.timeScale = 0f; // 게임 시간 정지
        // towerData <- shapeEquipment를 참조하여 도형 슬롯 값 받아오기
        int shapeSlotCount = _shapeEquipment.towerData.maxShapeSlots;
        // PerkManager에서 선별
        List<PerkData> options = PerkManager.Instance.GetPerkOptions(PERK_OPTIONS_COUNT, shapeSlotCount);
        // PerkManager에서 걸러진 퍽들을 UIManager로 표시
        UIManager.Instance.ShowPerkSelection(options, this);
    }

    public void SelectNApplyPerk(PerkData selectedPerk)
    {
        if (selectedPerk is ShapeData shapeData)
        {
            Debug.Log($"{selectedPerk.perkName} 선택");

            if (_inventory.HasPerk(shapeData))
            {
                _inventory.LevelUpPerk(shapeData);
            }
            else
            {
                _inventory.AddPerk(shapeData);
                _shapeEquipment.EquipNewShape(shapeData);
            }
        }
        else if (selectedPerk is UpgradePerkData upgradePerkData)
        {
            Debug.Log($"강화 퍽: {upgradePerkData.perkName} 선택");
            if (_inventory.HasPerk(upgradePerkData))
            {
                _inventory.LevelUpPerk(upgradePerkData);
            }
            else
            {
                _inventory.AddPerk(upgradePerkData);
            }
        }
        // ShapeEquipment 참조
        _shapeEquipment.UpdateAllWeaponStats();

        Time.timeScale = 1f;
    }
}
