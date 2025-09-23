using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    // 이동 패턴 enum 고려
    public enum MovementPattern { StraightToTower }

    [Header("적 스탯")]
    public string enemyName = "기본 적";
    public float maxHp = 20f;
    public float moveSpeed = 3f;
    public float damage = 10f;

    [Header("프리팹, 이동 패턴")]
    public GameObject enemyPrefab;
    public MovementPattern movementPattern = MovementPattern.StraightToTower;

    [Header("보상")]
    public int experienceReward = 5; // 처치 경험치

}
