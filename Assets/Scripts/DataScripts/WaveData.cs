using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyGroup
{
    public EnemyData enemyData;
    public int count;
}

[CreateAssetMenu(fileName = "New Wave Data", menuName = "Game Data/Wave Data")]
public class WaveData : ScriptableObject
{
    [Tooltip("웨이브 구성 적 목록")]
    public List<EnemyGroup> enemyGroups;

    [Tooltip("다음 웨이브까지 걸리는 시간(초)")]
    public float timeToNextWave = 10f;
}
