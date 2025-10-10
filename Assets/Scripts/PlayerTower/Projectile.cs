using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _damage;
    private float _speed;
    private float _lifetime;

    // 호밍 어택 관련 변수(유도탄)
    private Transform _homingTarget;
    private float _turnSpeed = 10f;

    // 관통 관련 변수(관통탄)
    private int _currentPierceCount; // 현재 남은 관통 횟수
    private List<Collider> _hitEnemies;

    // 투사체 초기회
    // 투사체 데이터, 최종 데미지, 최종 투사 스피드, 추적 타켓
    public void Initialize(ProjectileData data, float finalDamage, float finalSpeed, Transform homingTarget)
    {
        _damage = finalDamage;
        _speed = finalSpeed;
        _lifetime = data.lifetime;
        _homingTarget = homingTarget;

        _currentPierceCount = data.pierceCount;
        _hitEnemies = new List<Collider>();

        Destroy(gameObject, _lifetime);
    }

    void Update()
    {
        // 추적 목표가 존재 할 시에 작동
        if (_homingTarget != null)
        {
            Vector3 direction = (_homingTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);
        }
        
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (_hitEnemies.Contains(other)) return; // 이미 때린놈이면 넘어가기

            _hitEnemies.Add(other); // 처음 닿은 적이면 히트 리스트에 추가

            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
                _currentPierceCount--;
            }

            if (_currentPierceCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
