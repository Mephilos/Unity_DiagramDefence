using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private ProjectileData _projectileData;
    private float _finalDamage;
    private float _finalFireRate;
    private float _fireCooldown = 0f;
    private List<Transform> _firePoints;
    private FiringStrategy _firingStrategy;
    private int _nextFirePointIndex = 0;

    // 도형 데이타 초기화
    public void Initialize(ProjectileData projectileData, float finalDamage, float finalFireRate, List<Transform> firePoints, FiringStrategy firingStrategy)
    {
        _projectileData = projectileData;
        _finalDamage = finalDamage;
        _finalFireRate = finalFireRate;
        _fireCooldown = 0f;
        _firingStrategy = firingStrategy;
        _firePoints = firePoints;
    }

    void Update()
    {
        if (_projectileData == null || _firePoints == null || _firingStrategy == null) return; // 데이터 미설정시 아무것도 하지 않음.

        _fireCooldown -= Time.deltaTime;

        if (_fireCooldown <= 0f)
        {
            _firingStrategy.Fire(_projectileData, _firePoints, _finalDamage, ref _nextFirePointIndex);
            _fireCooldown = 1f / _finalFireRate;
        }
    }

    // void Fire()
    // {
    //     if (_projectileData.projectilePrefab == null) return;

    //     // 현제 투사체를 발사할 포인트를 가저옴
    //     Transform currentFirePoint = _firePoints[_nextFirePointIndex];
    //     // 투사체 프리팹 생성
    //     GameObject projectileObj = Instantiate(_projectileData.projectilePrefab, currentFirePoint.position, currentFirePoint.rotation);
    //     // 생성한 투사체에서 스크립트를 가져옴
    //     Projectile projectile = projectileObj.GetComponent<Projectile>();
    //     if (projectile != null)
    //     {
    //         // 가져온 데이터로 초기화
    //         projectile.Initialize(_projectileData, _finalDamage);
    //     }
    //     // 총구 인덱스 증가
    //     _nextFirePointIndex++;
    //     // 마지막 총구 인덱스에 다다랐을 경우 다시 0으로 리셋
    //     if (_nextFirePointIndex >= _firePoints.Count)
    //     {
    //         _nextFirePointIndex = 0;
    //     }
    // }
}
