using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Data", menuName = "Game Data/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [Header("총알 스탯")]
    public string ProjectileName = "기본";
    public float damage = 10f;
    public float speed = 20f;
    public float lifetime = 3f;

    [Header("관통 횟수(1이면 일반 투사체)")]
    public int pierceCount = 1;

    [Header("투사체 프리팹")]
    public GameObject projectilePrefab;
}
