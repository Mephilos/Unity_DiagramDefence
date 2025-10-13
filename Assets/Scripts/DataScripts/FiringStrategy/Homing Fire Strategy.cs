using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Homing Fire Strategy", menuName = "Game Data/Firing Strategies/Homing")]
public class HomingFireStrategy : FiringStrategy
{
    public float searchRadius = 20f;

    public LayerMask enemyLayer;

    public override void Fire(ProjectileData projectileData, List<Transform> firePoints, float finalDamage, ref int nextFirePointIndex, int addPierce)
    {
        if (firePoints == null || firePoints.Count == 0) return;

        Transform currentFirePoint = firePoints[nextFirePointIndex];

        Transform target = FindClosestEnemy(currentFirePoint.position);

        GameObject projectileObj = Instantiate(projectileData.projectilePrefab, currentFirePoint.position, currentFirePoint.rotation);

        int finalPierceCount = projectileData.pierceCount + addPierce;
        if (addPierce > 0 && projectileObj.GetComponent<PierceOpt>() == null)
        {
            projectileObj.AddComponent<PierceOpt>();
        }
        
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Initialize(projectileData, finalDamage, projectileData.speed, target, finalPierceCount);
        }

        nextFirePointIndex = (nextFirePointIndex + 1) % firePoints.Count;
    }

    // 가까운 적 찾는 함수
    private Transform FindClosestEnemy(Vector3 fromPosition)
    {
        // 원형(Sphere)로 탐색(콜라이더(물리 반응가능한 오브젝트)) 위치, 서치 지름, 서치 레이어
        Collider[] enemies = Physics.OverlapSphere(fromPosition, searchRadius, enemyLayer);

        Transform closestEnemy = null;
        float minDistance = float.MaxValue;

        // 탐색한 오브젝트 베열을 순회하면서 가장 가까운 오브젝트를 판별
        foreach (var enemyCollider in enemies)
        {
            float distance = Vector3.Distance(fromPosition, enemyCollider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemyCollider.transform;
            }
        }
        // 가장 가까운 적 Transform 리턴
        return closestEnemy;
    }
}
