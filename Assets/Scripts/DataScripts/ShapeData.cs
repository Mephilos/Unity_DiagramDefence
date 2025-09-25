using UnityEngine;

[CreateAssetMenu(fileName = "New Shape Data", menuName = "Game Data/Shape Data")]
public class ShapeData : ScriptableObject
{
    [Header("도형 정보")]
    public string shapeName = "기본 도형";
    [TextArea] public string description = "기본적인 탄 발사를 하는 도형입니다.";

    [Header("공격 능력")]
    public ProjectileData projectileData;
    public float fireRate = 1f; // 초당 발사 횟수

    [Header("회전 속도")]
    public float rotationSpeed = 30f;
    [Header("외형 프리팹")]
    public GameObject shapePrefab;
}
