using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sequential Fire Strategy", menuName = "Game Data/Firing Strategies/Sequential")]
public class SequentialFireStrategy : FiringStrategy
{
    public override void Fire(ProjectileData projectileData, List<Transform> firePoints, float finalDamage, ref int nextFirePointIndex)
    {
        if (firePoints == null) return;

        Transform currentFirePoint = firePoints[nextFirePointIndex];
        // 투사체 생성
        GameObject projectileObj = Instantiate(projectileData.projectilePrefab, currentFirePoint.position, currentFirePoint.rotation);
        // 투사체 데이터 가져오기
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            // ProjectileData 값으로 초기화
            projectile.Initialize(projectileData, finalDamage, projectileData.speed, null);
        }
        // 다음 발사 위치 인덱스
        nextFirePointIndex++;
        // 마지막 총구 판별 후 발사 위치 인덱스 초기화
        if (nextFirePointIndex >= firePoints.Count)
        {
            nextFirePointIndex = 0;
        }
    }
}
