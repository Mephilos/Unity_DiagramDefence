using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private ProjectileData _projectileData;
    private float _finalDamage;
    private float _finalFireRate;

    private float _fireCooldown = 0f;

    // 무기 데이타 초기화
    public void Initialize(ProjectileData projectileData, float finalDamage, float finalFireRate)
    {
        _projectileData = projectileData;
        _finalDamage = finalDamage;
        _finalFireRate = finalFireRate;
        _fireCooldown = 0f;
    }

    void Update()
    {
        if (_projectileData == null) return; // 데이터 미설정시 아무것도 하지 않음.

        _fireCooldown -= Time.deltaTime;

        if (_fireCooldown <= 0f)
        {
            Fire();
            _fireCooldown = 1f / _finalFireRate;
        }
    }

    void Fire()
    {
        if (_projectileData.projectilePrefab == null) return;
        // 투사체 프리팹 생성
        GameObject projectileObj = Instantiate(_projectileData.projectilePrefab, transform.position, transform.rotation);
        // 생성한 투사체에서 스크립트를 가져옴
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
        {   
            // 가져온 데이터로 초기화
            projectile.Initialize(_projectileData, _finalDamage);
        }
    }
}
