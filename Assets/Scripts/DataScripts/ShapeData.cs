using UnityEngine;

[CreateAssetMenu(fileName = "New Shape Data", menuName = "Game Data/Perks/Shape Data")]
public class ShapeData : PerkData
{
    [Header("공격 스텟 데이터")]
    public ProjectileData projectileData;
    public float fireRate = 1f; // 초당 발사 횟수

    [Header("발사 타입 데이터")]
    public FiringStrategy firingStrategy;
    [Header("회전 속도")]
    public float rotationSpeed = 30f;
    [Header("외형 프리팹")]
    public GameObject shapePrefab;

    [Header("합체 정보")]
    public ShapeData combinationPartner; // 합체 대상
    public ShapeData combinedPerk; // 합체 후 대상
}
