using UnityEngine;

// 투사체 인터페이스
public interface IProjectile
{
    void Initialize(Projectile projectile, ProjectileData data);

    // 프레임 마다 하는 부분을 정의
    void OnUpdate();

    // 투사체가 부딪혔을 때 정의
    void OnHit(Collider target, ref bool shouldDestroy);
    bool HasHit(Collider target);
}
