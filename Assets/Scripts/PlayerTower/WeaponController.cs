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
        if (_projectileData == null || _firePoints == null || _firingStrategy == null || _firePoints.Count == 0) return; // 데이터 미설정시 아무것도 하지 않음.

        _fireCooldown -= Time.deltaTime;

        if (_fireCooldown <= 0f)
        {
            int addPierce = TowerStatManager.Instance.AddPierceCount;
            _firingStrategy.Fire(_projectileData, _firePoints, _finalDamage, ref _nextFirePointIndex, addPierce);
            _fireCooldown = 1f / _finalFireRate;
        }
    }

    public List<Transform> GetFirePoints()
    {
        return _firePoints;
    }
}
