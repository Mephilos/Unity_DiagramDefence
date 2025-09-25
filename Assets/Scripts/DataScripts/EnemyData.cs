using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("적 스탯")]
    public string enemyName = "큐브";
    public float maxHp = 20f;
    public float moveSpeed = 3f;
    public float damage = 10f;

    [Header("프리팹, 이동 패턴")]
    public GameObject enemyPrefab;
    public MovementStrategy movementStrategy;

    [Header("보상")]
    public int experienceReward = 5; // 처치 경험치

}
