using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

// 관통 기능 옵션
public class PierceOpt : MonoBehaviour, IProjectile
{
    private int _currentPierceCount;
    private List<Collider> _hitEnemies;

    public void Initialize(Projectile projectile, ProjectileData data)
    {
        _currentPierceCount = data.pierceCount;
        _hitEnemies = new List<Collider>();
    }
    public void OnUpdate() { }
    public void OnHit(Collider target, ref bool shouldDestroy)
    {
        if (_hitEnemies.Contains(target)) return;

        _hitEnemies.Add(target);
        _currentPierceCount--;

        if (_currentPierceCount <= 0) shouldDestroy = true;
    }
    public bool HasHit(Collider target)
    {
        return _hitEnemies.Contains(target);
    }
}
