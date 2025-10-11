using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Projectile : MonoBehaviour
{
    private float _damage;
    private float _speed;

    private List<IProjectile> _iProjectile;

    public Transform Target { get; private set; }

    // void Awake()
    // {
    //     _iProjectile = new List<IProjectile>(GetComponents<IProjectile>());
    // }
    // 투사체 초기회
    // 투사체 데이터, 최종 데미지, 최종 투사 스피드, 추적 타켓
    public void Initialize(ProjectileData data, float finalDamage, float finalSpeed, Transform homingTarget, int finalPierceCount)
    {
        _damage = finalDamage;
        _speed = finalSpeed;
        Target = homingTarget;

        _iProjectile = new List<IProjectile>(GetComponents<IProjectile>());

        foreach (var iProjectile in _iProjectile)
        {
            iProjectile.Initialize(this, finalPierceCount);
        }

        Destroy(gameObject, data.lifetime);
    }

    void Update()
    {
        foreach (var iProjectile in _iProjectile)
        {
            iProjectile.OnUpdate(); // 각 기능들의 OnUpdate 함수를 실행
        }
    
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ProjectConstants.ENEMY_TAG))
        {
            if (_iProjectile.Any(b => b.HasHit(other))) return;

            other.GetComponent<EnemyController>()?.TakeDamage(_damage);

            bool shouldSurvive = false;

            foreach (var iProjectile in _iProjectile)
            {
                iProjectile.OnValidHit(other, ref shouldSurvive);
            }

            if (!shouldSurvive)
            {
                Destroy(gameObject);
            }
        }
    }
}
