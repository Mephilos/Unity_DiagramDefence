using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Graphs;
using Codice.CM.Client.Differences;
using System.Runtime.InteropServices.WindowsRuntime;
public class PlayerTowerController : MonoBehaviour
{
    private const int PERK_OPTIONS_COUNT = 3;

    [Header("타워 설정")]
    public TowerData towerData;

    [Header("플레이어 HP")]
    public float maxHp = 100f;

    [Header("터렛 설정")]
    public Transform turretTransform; // 터렛 부분
    public LayerMask groundLayerMask;

    [Header("무기 시스템 설정")]
    [Tooltip("무기 장착 리스트")]
    public List<Transform> weaponSlots;

    [Tooltip("터렛 데이터")]
    public ShapeData defaultTurretData;
    [Tooltip("테스트용")]
    // public List<PerkData> testPerkOptions;
    
    [Header("레벨, 요구 경험치")]
    [SerializeField] private int level = 1;
    [SerializeField] private float currentExperience = 0f;
    [SerializeField] private float requiredExperience = 100f;
    private PerkInventory _inventory;
    // private int equippedCount = 0; // 장착된 도형 카운트 -> 인벤토리 관리로 변경
    private float _currentHp;
    private Camera _mainCamera;

    void Awake()
    {
        _inventory = GetComponent<PerkInventory>();
    }
    void Start()
    {
        _currentHp = maxHp;
        _mainCamera = Camera.main;

        if (turretTransform == null) Debug.LogError($"[{gameObject}] 터렛 트렌스폼 설정 필요");

        InitializeDefaultTurret();

        PerkManager.Instance.RegisterPlayerInventory(_inventory);
        TowerStatManager.Instance.RegisterPlayerInventory(_inventory);
    }

    void Update()
    {
        HandleTurretRotation();
    }

    // 경험치 획득
    public void GainExperience(float amount)
    {
        currentExperience += amount;
        Debug.Log($"[{gameObject}] 경험치 획득 {amount} / 현재 경험치: {currentExperience} / {requiredExperience}");

        while (currentExperience >= requiredExperience)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentExperience -= requiredExperience; // 현재 경험치에서 이전 레벨의 필요 경험치 만큼 빼서 남은 경험치는 보존되도록
        level++;

        requiredExperience *= 1.2f; // 요구 경험치 증가를 위한 요구 경험치 증가 계수

        Debug.LogWarning($"레벨 업. 현재 레벨: {level} / 다음 레벨 요구 경험치: {requiredExperience}");

        Time.timeScale = 0f; // 게임 시간 정지
        // 터렛 슬롯을 제외한 도형슬롯 갯수
        int shapeSlotCount = weaponSlots.Count - 1;
        // PerkManager에서 선별
        List<PerkData> options = PerkManager.Instance.GetPerkOptions(PERK_OPTIONS_COUNT, shapeSlotCount);
        // PerkManager에서 걸러진 퍽들을 UIManager로 표시
        UIManager.Instance.ShowPerkSelection(options, this);
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        Debug.LogWarning($"플레이어 hp 감소: {_currentHp} / {maxHp}");

        if (_currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("게임 오버");
        Destroy(gameObject);
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
            turretFirePoints,
            defaultTurretData.firingStrategy
        );
        // equippedCount = 0;
    
    }
    private void UpdateTurretStat()
    {
        WeaponController turretWeapon = weaponSlots[0].GetComponent<WeaponController>();
        if (turretWeapon != null)
        {
            // 최종 터렛 데미지 계산 (기본 터렛 데미지 * 버프 값)
            float finalDamage = defaultTurretData.projectileData.damage * TowerStatManager.Instance.TotalDamageMultiplier;
            float finalFireRate = defaultTurretData.fireRate * TowerStatManager.Instance.TotalFireRateMultiplier;

            // 가져온 웨폰 컨트롤러를 기본 터렛 데이터로 초기화
            turretWeapon.Initialize(
                defaultTurretData.projectileData,
                finalDamage,
                finalFireRate,
                turretWeapon.GetFirePoints(),
                defaultTurretData.firingStrategy
            );
        }
    }

    void HandleTurretRotation()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition); // 카메라에서 마우스 포지션으로 레이 발사

        // 레이 충동 위치 확인
        // 사거리 100, 충돌 레이어 설정(groundLayerMask)
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayerMask))
        {

            Vector3 targetPoint = hit.point;
            Vector3 direction = targetPoint - turretTransform.position; // 방향 백터 계산 (터렛 위치에서 목표 지점)

            direction.y = 0; // 터렛 y축 무시

            // 계산된 방향으로 방향 조정
            if (direction != Vector3.zero)
            {
                turretTransform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    // private void HandlePerkSelection()
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha1) && testPerkOptions.Count >= 1)      SelectPerk(testPerkOptions[0]);
    //     else if (Input.GetKeyDown(KeyCode.Alpha2) && testPerkOptions.Count >= 2) SelectPerk(testPerkOptions[1]);
    //     else if (Input.GetKeyDown(KeyCode.Alpha3) && testPerkOptions.Count >= 3) SelectPerk(testPerkOptions[2]);
    //     else if (Input.GetKeyDown(KeyCode.Alpha3) && testPerkOptions.Count >= 4) SelectPerk(testPerkOptions[3]);
    // }

    // 퍽 카드를 선택 했을 때 호출 되는 함수
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
                EquipNewShape(shapeData);
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
        UpdateAllWeaponStats();

        Time.timeScale = 1f;
    }

    private void UpdateAllWeaponStats()
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
    private void EquipNewShape(ShapeData shapeData)
    {
        // 장착된 도형의 갯수를 인벤토리로 부터 받아옴.
        int equippedCount = _inventory.GetEquippedShapeCount();
        // 장착가능 슬롯에서 터렛 제외
        int availableSlots = weaponSlots.Count - 1;
        // 장착한 슬롯 확인 (슬롯이 풀이면 함수 종료)
        if (equippedCount >= availableSlots)
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
                float finalRotationSpeed = shapeData.rotationSpeed * bonus.rotationSpeedMultiplier * TowerStatManager.Instance.TotalRotationSpeedMultiplier;
                ShapeRotator rotator = shapeInstance.AddComponent<ShapeRotator>();
                rotator.Initialize(finalRotationSpeed);
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

        // 레벨업 보너스 계산.
        float levelUpDamageBonus = (shapeStatus.currentLevel - 1) * shapeData.damagePerLevel;
        float levelUpFireRatebonus = (shapeStatus.currentLevel - 1) * shapeData.fireRatePerLevel;
        // 최족 스텟 계산(기본 스텟 + 레벨업 보너스) * 플로어 보너스 * 글로벌 스텟 보너스(강화 퍽)
        float finalDamage = (shapeData.projectileData.damage + levelUpDamageBonus) * bonus.damageMultiplier * TowerStatManager.Instance.TotalDamageMultiplier;
        float finalFireRate = (shapeData.fireRate + levelUpFireRatebonus) * bonus.fireRateMultiplier * TowerStatManager.Instance.TotalFireRateMultiplier;
        // 슬롯에 장착된 도형의 웨폰 컨트롤러 가져움
        WeaponController weapon = slot.GetComponent<WeaponController>();
        if (weapon != null)
        {
            ShapeInfo shapeInfo = slot.GetComponentInChildren<ShapeInfo>();
            List<Transform> firePoints = shapeInfo.firePoints;
            // 도형 데이터 추가, 데이터 최신 데이터로 초기화
            weapon.Initialize(shapeData.projectileData, finalDamage, finalFireRate, firePoints, shapeData.firingStrategy);
        }
    }
}
