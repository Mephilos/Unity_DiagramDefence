using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun Fire Strategy", menuName = "Game Data/Firing Strategies/Shotgun")]
public class ShotgunFireStrategy : FiringStrategy
{
    [Header("산탄 설정")]
    public int pelletCount = 5;
    public float spreadAngle = 30f;

    [Range(0f, 1f)]
    public float speedVariation = 0.2f;

    public static float CalculateDamagePerPellet(float totalDamage, int count)
    {
        return (count > 0) ? totalDamage / count : 0;
    }
    public override void Fire(ProjectileData projectileData, List<Transform> firePoints, float finalDamage, ref int nextFirePointIndex, int addPierce)
    {
        if (firePoints == null || firePoints.Count == 0) return;

        float damagePerPellet = CalculateDamagePerPellet(finalDamage, pelletCount);

        float baseSpeed = projectileData.speed;

        foreach (Transform firePoint in firePoints)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                Quaternion baseRotation = firePoint.rotation;

                float randomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
                Quaternion spreadRotation = Quaternion.Euler(0, randomAngle, 0);

                Quaternion finalRotation = baseRotation * spreadRotation;

                float finalSpeed = baseSpeed * (1 + Random.Range(-speedVariation, speedVariation));

                GameObject projectileObj = Instantiate(projectileData.projectilePrefab, firePoint.position, finalRotation);

                // 최종 관통 횟수 계산
                int finalPierceCount = projectileData.pierceCount + addPierce;
                // 관통 옵션이 있고, 생성된 오브젝트에 PierceOpt가 없을 경우 동적으로 붙여줌
                if (addPierce > 0 && projectileObj.GetComponent<PierceOpt>() == null)
                {
                    projectileObj.AddComponent<PierceOpt>();
                }

                Projectile projectile = projectileObj.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.Initialize(projectileData, damagePerPellet, finalSpeed, null, finalPierceCount);
                }
            }
        }
    }
}
