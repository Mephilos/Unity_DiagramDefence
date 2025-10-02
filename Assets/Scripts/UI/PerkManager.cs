using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PerkManager : MonoBehaviour
{
    public static PerkManager Instance { get; private set; }

    public List<PerkData> masterPerkList; // 모든 퍽을 관리하는 리스트

    private PerkInventory _playerInventory; // 플레이어의 인벤토리 참조

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // PlayerTowerController가 참조할 함수
    public void RegisterPlayerInventory(PerkInventory inventory)
    {
        _playerInventory = inventory;
    }

    // 레벨 업시 호출될 함수
    public List<PerkData> GetPerkOptions(int count, int maxShapeSlots)
    {
        if (_playerInventory == null)
        {
            Debug.LogError($"{gameObject} 플레이어 인벤토리가 등록되지 않았습니다");
            return new List<PerkData>();
        }
        // 현재 등장 가능한 퍽들만 걸러내서 리스트 작성
        List<PerkData> eligiblePerks = new List<PerkData>();
        foreach (PerkData perk in masterPerkList) // 모든 리스트 순회
        {
            // 퍽을 소유 중이고, 퍽이 최대 레벨인가
            PerkStatus status = _playerInventory.GetStatus(perk);
            if (status != null && status.currentLevel >= perk.maxLevel)
            {
                continue; // 맞다면 건너 뛰기
            }
            // 퍽이 도형퍽인가?
            if (perk is ShapeData shape)
            {
                // 도형 슬롯이 꽉 차있고, 가지고 있지 않은 도형인가?
                if (_playerInventory.IsShapeSlotsFull(maxShapeSlots) && !_playerInventory.HasPerk(shape))
                {
                    continue; // 맞다면 건너뛰기
                }
            }
            // 모든 필터를 통과한 퍽은 리스트에 등록
            eligiblePerks.Add(perk);
        }
        // 리스트를 섞음
        List<PerkData> shuffledPerks = eligiblePerks.OrderBy(p => Random.value).ToList();
        // 리스트 앞에서 부터 count 개수만큼 가져오기
        return shuffledPerks.Take(count).ToList();
    }
}
