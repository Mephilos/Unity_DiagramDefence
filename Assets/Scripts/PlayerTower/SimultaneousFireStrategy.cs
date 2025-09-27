using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Simultaneous Fire Strategy", menuName = "Game Data/Firing Strategies/Simultaneous")]
public class SimultaneousFireStrategy : FiringStrategy
{
    public override void Fire(ProjectileData projectileData, List<Transform> firePoints, float finalDamage, ref int nextFirePointIndex)
    {
        if (firePoints == null) return;

        foreach (Transform firePoint in firePoints)
        {
            GameObject projectileObj = Instantiate(projectileData.projectilePrefab, firePoint.position, firePoint.rotation);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Initialize(projectileData, finalDamage);
            }
        }
    }
}
