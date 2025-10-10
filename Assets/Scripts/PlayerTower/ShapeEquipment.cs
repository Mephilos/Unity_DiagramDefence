using UnityEngine;
using System.Collections.Generic;

public class ShapeEquipment : MonoBehaviour
{
    [Header("타워 설정")]
    public TowerData towerData;

    [Header("무기 시스템 설정")]
    [Tooltip("무기 장착 리스트")]
    public List<Transform> weaponSlots;

    [Tooltip("터렛 데이터")]
    public ShapeData defaultTurretData;

    private PerkInventory _inventory;

    void Awake()
    {
        _inventory = GetComponent<PerkInventory>();
    }

    void Start()
    {
        InitializeDefaultTurret();
    }

    private void InitializeDefaultTurret()
    {
        if (weaponSlots.Count == 0 || defaultTurretData == null)
        {
            Debug.LogError($"{gameObject}터렛 설정 필요");
            return;
        }
        // 0번슬롯은 터렛 거기에 있는 터렛의 웨폰컨트롤러 가져오기
        WeaponController turretWeapon = weaponSlots[0].GetComponent<WeaponController>();

        if (turretWeapon == null)
        {
            turretWeapon = weaponSlots[0].gameObject.AddComponent<WeaponController>();
        }

        List<Transform> turretFirePoints = new List<Transform> { weaponSlots[0] };

        // 가져온 웨폰 컨트롤러를 기본 터렛 데이터로 초기화
        turretWeapon.Initialize(
            defaultTurretData.projectileData,
            defaultTurretData.projectileData.damage,
            defaultTurretData.fireRate,
            defaultTurretData.projectileData.speed,
            turretFirePoints,
            defaultTurretData.firingStrategy
        );
    }
    public void UpdateAllWeaponStats()
    {
        // 타워 스탯 재계산
        TowerStatManager.Instance.RecalculateStats();
        // 퍽 인벤토리의 있는 장착된 도형 퍽 스탯을 업데이트 순회
        foreach (PerkStatus shapeStatus in _inventory.GetEquippedShapes())
        {
            UpdateSingleWeaponStat(shapeStatus);
        }
        // 터렛 스텟 업데이트
        UpdateTurretStat();
    }
    private void UpdateTurretStat()
    {
        WeaponController turretWeapon = weaponSlots[0].GetComponent<WeaponController>();
        if (turretWeapon != null)
        {
            // 최종 터렛 데미지 계산 (기본 터렛 데미지 * 버프 값)
            float finalDamage = defaultTurretData.projectileData.damage * TowerStatManager.Instance.TotalDamageMultiplier;
            float finalFireRate = defaultTurretData.fireRate * TowerStatManager.Instance.TotalFireRateMultiplier;
            float finalProjectileSpeed = defaultTurretData.projectileData.speed * TowerStatManager.Instance.TotalProjectileSpeedMultiplier;
            // 가져온 웨폰 컨트롤러를 기본 터렛 데이터로 초기화
            turretWeapon.Initialize(
                defaultTurretData.projectileData,
                finalDamage,
                finalFireRate,
                finalProjectileSpeed,
                turretWeapon.GetFirePoints(),
                defaultTurretData.firingStrategy
            );
        }
    }
    
    public void EquipNewShape(ShapeData shapeData)
    {
        // 장착된 도형의 갯수를 인벤토리로 부터 받아옴.
        int equippedCount = _inventory.GetEquippedShapeCount();
        // 장착가능 슬롯에서 터렛 제외
        int availableSlots = weaponSlots.Count - 1;
        // 장착한 슬롯 확인 (슬롯이 풀이면 함수 종료)
        if (equippedCount > availableSlots)
        {
            Debug.LogWarning("무기 슬롯 풀");
            _inventory.RemovePerk(shapeData);
            return;
        }
        int floorIndex = equippedCount - 1;
        int slotIndex = floorIndex + 1; // 터렛 제외

        if (floorIndex >= towerData.floorBonuses.Count)
        {
            Debug.LogError($"{floorIndex}층 보너스가 정의 되지 않음.");
            return;
        }

        // 빈 슬롯 가져오기
        Transform slot = weaponSlots[slotIndex];
        // 플로어 보너스 가져오기
        FloorBonus bonus = towerData.floorBonuses[floorIndex];

        // 해당 슬롯에 무기의 프리팹을 생성
        if (shapeData.shapePrefab != null)
        {
            GameObject shapeInstance = Instantiate(shapeData.shapePrefab, slot.position, slot.rotation, slot);

            shapeInstance.transform.localScale = new Vector3(
                shapeInstance.transform.localScale.x * bonus.visualScale,
                shapeInstance.transform.localScale.y,
                shapeInstance.transform.localScale.z * bonus.visualScale
            );
            // 장착된 도형퍽이 회전 할지 말지 확인 후 회전 작동
            if (shapeData.rotationSpeed != 0f)
            {
                ShapeRotator rotator = shapeInstance.AddComponent<ShapeRotator>();
                rotator.Initialize(shapeData.rotationSpeed); // 기본으로 초기화
            }
        }
        // 슬롯에 있는 오브젝트(도형)에 웨폰 컨트롤러가 없을시에 추가
        if (slot.GetComponent<WeaponController>() == null)
        {
            slot.gameObject.AddComponent<WeaponController>();
        }
    }
    private void UpdateSingleWeaponStat(PerkStatus shapeStatus)
    {
        // 형변환 실패시 함수 탈출(데이터가 도형 데이터가 아닐경우를 위한 안전장치)
        if (!(shapeStatus.perkData is ShapeData shapeData)) return;
        // 도형이 몇 번쩨 플로어에 장착된 도형인지 인덱스 정보 가져오기
        int floorIndex = _inventory.GetEquippedShapes().IndexOf(shapeStatus);
        int slotIndex = floorIndex + 1; // 0번 슬롯은 터렛

        if (slotIndex >= weaponSlots.Count)
        {
            Debug.LogError($"{gameObject} 무기 슬롯 인덱스 범위 확인");
            return;
        }

        Transform slot = weaponSlots[slotIndex];
        if (floorIndex >= towerData.floorBonuses.Count)
        {
            Debug.LogError($"{gameObject} 정의된 플로어 보너스 수를 오버");
            return;
        }
        // 플로어 보너스 가져오기
        FloorBonus bonus = towerData.floorBonuses[floorIndex];

        ShapeRotator rotator = slot.GetComponentInChildren<ShapeRotator>();
        if (rotator != null)
        {
            float finalRotationSpeed = shapeData.rotationSpeed * bonus.rotationSpeedMultiplier * TowerStatManager.Instance.TotalRotationSpeedMultiplier;
            rotator.Initialize(finalRotationSpeed);
        }
        // 레벨업 보너스 계산.
        float levelUpDamageBonus = (shapeStatus.currentLevel - 1) * shapeData.damagePerLevel;
        float levelUpFireRatebonus = (shapeStatus.currentLevel - 1) * shapeData.fireRatePerLevel;
        // 최족 스텟 계산(기본 스텟 + 레벨업 보너스) * 플로어 보너스 * 글로벌 스텟 보너스(강화 퍽)
        float finalDamage = (shapeData.projectileData.damage + levelUpDamageBonus) * bonus.damageMultiplier * TowerStatManager.Instance.TotalDamageMultiplier;
        float finalFireRate = (shapeData.fireRate + levelUpFireRatebonus) * bonus.fireRateMultiplier * TowerStatManager.Instance.TotalFireRateMultiplier;
        float finalProjectileSpeed = shapeData.projectileData.speed * TowerStatManager.Instance.TotalProjectileSpeedMultiplier; 

        // 슬롯에 장착된 도형의 웨폰 컨트롤러 가져움
        WeaponController weapon = slot.GetComponent<WeaponController>();
        if (weapon != null)
        {
            ShapeInfo shapeInfo = slot.GetComponentInChildren<ShapeInfo>();
            List<Transform> firePoints = shapeInfo.firePoints;
            // 도형 데이터 추가, 데이터 최신 데이터로 초기화
            weapon.Initialize(shapeData.projectileData, finalDamage, finalFireRate, finalProjectileSpeed, firePoints, shapeData.firingStrategy);
        }
    }
}
