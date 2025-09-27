using System.Collections.Generic;
using UnityEngine;

public abstract class FiringStrategy : ScriptableObject
{   
    // 투사체 설정, 투사차 발사 포인트, 데미지, 다음 발사 포지션 인덱스
    public abstract void Fire(ProjectileData projectileData, List<Transform> firePoints, float finalDamage, ref int nextFirePointIndex);
}
