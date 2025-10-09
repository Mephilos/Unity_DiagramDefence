using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _damage;
    private float _speed;
    private float _lifetime;

    private Transform _homingTarget;
    private float _turnSpeed = 10f;

    // 투사체 초기회
    // 투사체 데이터, 최종 데미지, 최종 투사 스피드, 추적 타켓
    public void Initialize(ProjectileData data, float finalDamage, float finalSpeed, Transform homingTarget)
    {
        _damage = finalDamage;
        _speed = finalSpeed;
        _lifetime = data.lifetime;
        _homingTarget = homingTarget;

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
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}
