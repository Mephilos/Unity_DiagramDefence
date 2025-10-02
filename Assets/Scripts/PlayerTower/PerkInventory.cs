using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Codice.Client.Commands;

[System.Serializable]
public class PerkStatus
{
    public PerkData perkData; // 어떤 퍽인가
    public int currentLevel; // 현제 강화 레벨은

    public PerkStatus(PerkData data)
    {
        perkData = data;
        currentLevel = 1; // 초기 레벨
    }
}

public class PerkInventory : MonoBehaviour
{
    [SerializeField] private List<PerkStatus> ownedPerks = new List<PerkStatus>(); // 소유하고 있는 모든 퍽 리스트
    [SerializeField] private List<PerkStatus> equippedShapes = new List<PerkStatus>(); // 장착중인 도형 퍽 리스트

    public void AddPerk(PerkData newPerk)
    {
        if (HasPerk(newPerk))
        {
            // 기존의 가지고 있는 퍽을 새로운 퍽으로 받을 경우(이슈 사항)
            return;
        }

        PerkStatus newStatus = new PerkStatus(newPerk);
        ownedPerks.Add(newStatus);
        // 새로 얻은 퍽이 도형이면 equippedShapes에도 추가
        if (newPerk is ShapeData)
        {
            equippedShapes.Add(newStatus);
        }
    }

    public void LevelUpPerk(PerkData perkToLevelUp)
    {
        PerkStatus status = GetStatus(perkToLevelUp);
        if (status != null && status.currentLevel < status.perkData.maxLevel)
        {
            status.currentLevel++;
            Debug.Log($"{perkToLevelUp.perkName} 퍽 레벨 업 {status.currentLevel}");
        }
    }

    // 소유 퍽을 확인하는 함수
    public bool HasPerk(PerkData perk)
    {
        // ownerPerks 리스트 안에 perkData가 찾는 perk과 같은게 있는 지 확인
        return ownedPerks.Any(p => p.perkData == perk);
    }
    // 퍽의 정보를 가져오는 함수
    public PerkStatus GetStatus(PerkData perk)
    {
        return ownedPerks.FirstOrDefault(p => p.perkData == perk);
    }
    //도형 개수 알려주는 함수
    public int GetEquippedShapeCount()
    {
        return equippedShapes.Count;
    }

    // 특정 도형이 최대 레벨인지 확인 하는 함수
    public bool IsShapeMaxLevel(ShapeData shape)
    {
        PerkStatus status = GetStatus(shape);
        return status != null && status.currentLevel >= status.perkData.maxLevel;
    }
    public bool IsShapeSlotsFull(int maxSlots)
    {
        return equippedShapes.Count >= maxSlots;
    }
    public List<PerkStatus> GetAllOwnedPerks()
    {
        return ownedPerks;
    }

    public List<PerkStatus> GetEquippedShapes()
    {
        return equippedShapes;
    }
    // 장착 실패시, 변경시 인벤토리에서 퍽을 제거하기 위한 함수
    public void RemovePerk(PerkData perkToRemove)
    {
        PerkStatus statusToRemove = GetStatus(perkToRemove);
        if (statusToRemove != null)
        {
            ownedPerks.Remove(statusToRemove);
            if (perkToRemove is ShapeData)
            {
                equippedShapes.Remove(statusToRemove);
            }
        }
    }
}
