using UnityEngine;

// 유도 기능 옵션
public class HomingOpt : MonoBehaviour, IProjectile
{
    private Projectile _projectile;
    private Transform _target;
    private float _turnSpeed = 10f;

    public void Initialize(Projectile projectile, int pierceCount)
    {
        _projectile = projectile;
        _target = projectile.Target;
    }

    public void OnUpdate()
    {
        if (_target == null) return; // 목표 없으면 종료

        Vector3 direction = (_target.position - _projectile.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _projectile.transform.rotation = Quaternion.Slerp(_projectile.transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);
    }

    public void OnValidHit(Collider target, ref bool shouldSurvive) { }
    public bool HasHit(Collider target) { return false; }
}
