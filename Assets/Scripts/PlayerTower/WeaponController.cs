using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("투사체 설정")]
    public ProjectileData projectileData;
    public float fireRate = 2f; // 초당 발사 속도

    private float _fireCooldown = 0f;

    void Update()
    {
        _fireCooldown -= Time.deltaTime;

        if (_fireCooldown <= 0f)
        {
            Fire();
            _fireCooldown = 1f / fireRate;
        }
    }

    void Fire()
    {
        if (projectileData == null || projectileData.projectilePrefab == null)
        {
            Debug.LogWarning($"[{gameObject}] 투사체 데이터, 프리팹 설정 확인");
            return;
        }
        // 투사체 프리팹 생성
        GameObject projectileObj = Instantiate(projectileData.projectilePrefab, transform.position, transform.rotation);
        // 생성한 투사체에서 스크립트를 가져옴
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
        {   
            // 가져온 데이터로 초기화
            projectile.Initialize(projectileData);
        }
    }
}
