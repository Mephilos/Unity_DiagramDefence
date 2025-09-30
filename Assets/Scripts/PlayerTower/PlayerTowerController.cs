using System.Collections.Generic;
using UnityEngine;

public class PlayerTowerController : MonoBehaviour
{
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
    public List<PerkData> testPerkOptions;

    private PerkInventory _inventory;
    // private int equippedCount = 0; // 장착된 도형 카운트 -> 인벤토리 관리로 변경

    [Header("레벨, 요구 경험치")]
    [SerializeField] private int level = 1;
    [SerializeField] private float currentExperience = 0f;
    [SerializeField] private float requiredExperience = 100f;

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
    }

    void Update()
    {
        HandleTurretRotation();
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
    private void InitializeDefaultTurret()
    {
        if (weaponSlots.Count == 0 || defaultTurretData == null)
        {
            Debug.LogError($"{gameObject}터렛 설정 필요");
            return;
        }
        // 0번슬롯은 터렛 거기에 있는 터렛의 웨폰컨트롤러 가져오기
        WeaponController turretWeapon = weaponSlots[0].GetComponent<WeaponController>();
        if (turretWeapon != null)
        {
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

        UIManager.Instance.ShowPerkSelection(testPerkOptions, this);
    }

    // private void HandlePerkSelection()
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha1) && testPerkOptions.Count >= 1)      SelectPerk(testPerkOptions[0]);
    //     else if (Input.GetKeyDown(KeyCode.Alpha2) && testPerkOptions.Count >= 2) SelectPerk(testPerkOptions[1]);
    //     else if (Input.GetKeyDown(KeyCode.Alpha3) && testPerkOptions.Count >= 3) SelectPerk(testPerkOptions[2]);
    //     else if (Input.GetKeyDown(KeyCode.Alpha3) && testPerkOptions.Count >= 4) SelectPerk(testPerkOptions[3]);
    // }

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

        Time.timeScale = 1f;
    }

    private void EquipNewShape(ShapeData shapeData)
    {
        // 장착된 도형의 갯수를 인벤토리로 부터 받아옴.
        int equippedCount = _inventory.GetEquippedShapeCount();
        // 장착가능 슬롯에서 터렛 제외
        int availableSlots = weaponSlots.Count - 1;
        // 장착한 슬롯 확인
        if (equippedCount >= availableSlots)
        {
            Debug.LogWarning("무기 슬롯 풀");
            return;
        }
        int floorIndex = equippedCount - 1;
        int slotIndex = floorIndex + 1; // 터렛 제외
        // 빈 슬롯 가져오기
        Transform slot = weaponSlots[slotIndex];
        // 플로어 보너스 가져오기
        FloorBouns bouns = towerData.floorBouns[floorIndex];

        float finalDamage = shapeData.projectileData.damage * bouns.damageMultiplier;
        float finalFireRate = shapeData.fireRate * bouns.fireRateMultiplier;
        // 총구위치를 저장할 리스트
        List<Transform> shapeFirePoints = new List<Transform>();
        // 해당 슬롯에 무기의 프리팹을 생성
        if (shapeData.shapePrefab != null)
        {
            GameObject shapeInstance = Instantiate(shapeData.shapePrefab, slot.position, slot.rotation, slot);
            shapeInstance.transform.localScale = new Vector3(
                                                             shapeInstance.transform.localScale.x * bouns.visualScale,
                                                             0.1f,
                                                             shapeInstance.transform.localScale.z * bouns.visualScale
                                                            );
            // 장착된 도형퍽이 회전 할지 말지 확인 후 회전 작동
            if (shapeData.rotationSpeed != 0f)
            {
                float finalRotationSpeed = shapeData.rotationSpeed * bouns.rotationSpeedMultiplier;
                ShapeRotator rotator = shapeInstance.AddComponent<ShapeRotator>();
                rotator.Initialize(finalRotationSpeed);
            }

            ShapeInfo shapeInfo = shapeInstance.GetComponent<ShapeInfo>();
            if (shapeInfo != null)
            {
                shapeFirePoints = shapeInfo.firePoints;
            }
        }
        
        if (shapeFirePoints.Count == 0)
        {
            Debug.LogWarning($"[{gameObject}] 도형 총구 위치 설정 필요");
            return;
        }

        // 기존의 컨트롤러가 존제하면 가져옴
        WeaponController newWeapon = slot.GetComponent<WeaponController>();
        // 없을 경우
        if (newWeapon == null)
        {
            // 해당되는 슬롯에 WeaponController 추가
            newWeapon = slot.gameObject.AddComponent<WeaponController>();
        }
        // WeaponController에 도형 데이터 추가 및 초기화
        newWeapon.Initialize(shapeData.projectileData, finalDamage, finalFireRate, shapeFirePoints, shapeData.firingStrategy);
    }
    void Die()
    {
        Debug.Log("게임 오버");
        Destroy(gameObject);
    }
}
