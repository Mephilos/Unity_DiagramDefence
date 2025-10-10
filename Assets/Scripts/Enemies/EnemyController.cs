using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    private EnemyData _enemyData;
    private float _currentHp;
    private Transform _target;

    private PlayerStatus _playerStatus;
    private float _totalAngleRotated = 0f; // 회전 추적 변수
    public void Initialize(EnemyData data, Transform target)
    {
        _enemyData = data;
        _target = target;
        _currentHp = _enemyData.maxHp;

        if (_target != null) _playerStatus = _target.GetComponent<PlayerStatus>(); // playerController 캐싱
    }

    void Update()
    {
        HandleMovement();

        if (_totalAngleRotated >= 360f)
        {
            Explode();
        }
    }
    public void TakeDamage(float damageAmount)
    {
        _currentHp -= damageAmount;
        if (_currentHp <= 0)
        {
            Die();
        }
    }
    private void HandleMovement()
    {
        // 이동 패턴을 전략 패턴으로 관리(MovementStrategy를 상속하여 작성)
        if (_enemyData.movementStrategy != null)
        {
            Vector3 moveDirection = _enemyData.movementStrategy.Move(transform, _target, _enemyData.moveSpeed, ref _totalAngleRotated);

            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
    }

    // 자폭 공격용 매서드
    private void Explode()
    {
        if (_playerStatus != null)
        {
            _playerStatus.TakeDamage(_enemyData.damage);
        }

        Destroy(gameObject);
    }

    // 죽음 매서드
    private void Die()
    {
        // 플레이어 확인후 플레이어 컨트롤러에 경험치 전달
        if (_playerStatus != null)
        {
            _playerStatus.GainExperience(_enemyData.experienceReward);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ProjectConstants.PLAYER_TAG))
        {
            if (_playerStatus != null)
            {
                _playerStatus.TakeDamage(_enemyData.damage);
            }
            Destroy(gameObject);
        }
    }
}
