using UnityEngine;

public class PlayerTowerController : MonoBehaviour
{
    [Header("플레이어 HP")]
    public float maxHp = 100f;

    [Header("터렛 설정")]
    public Transform turretTransform; // 터렛 부분
    public LayerMask groundLayerMask;

    private float _currentHp;
    private Camera _mainCamera;

    void Start()
    {
        _currentHp = maxHp;
        _mainCamera = Camera.main;

        if (turretTransform == null) Debug.LogError($"[{gameObject}] 터렛 트렌스폼 설정 필요");

    }

    void Update()
    {
        HandleTurretRotation();
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
}
