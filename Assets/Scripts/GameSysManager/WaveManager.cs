using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // 웨이브 진행 관리용 매니저
    // 싱글톤
    public static WaveManager Instance { get; private set; }

    [Header("스테이지 설정")]
    public StageData currentStage;

    [Header("참조")]
    public EnemySpawner enemySpawner;

    private int _currentWaveIndex = -1;
    private Coroutine _waveCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    
    void Start()
    {
        if (currentStage == null || enemySpawner == null)
        {
            Debug.LogError($"{gameObject} 스테이지 데이터, EnemySawner 설정 필요");
            return;
        }

        StartNextWave();
    }

    public void StartNextWave()
    {
        // 웨이브 인덱스 증가(웨이브 진행시작시)
        _currentWaveIndex++;

        // 다음 웨이브가 남아있는지 확인
        if (_currentWaveIndex < currentStage.waves.Count)
        {
            // 진행중인 코루틴이 있으면 코루틴 중지
            if (_waveCoroutine != null) StopCoroutine(_waveCoroutine);

            // 아니라면 새로운 웨이브 코루틴 시작
            _waveCoroutine = StartCoroutine(RunWave(currentStage.waves[_currentWaveIndex]));
        }
        else // 다음 진행 웨이브가 없다면 승리
        {
            GameManager.Instance.EndGame(true);
        }
    }

    // 1웨이브씩 진행 하는 코루틴
    private IEnumerator RunWave(WaveData waveData)
    {
        Debug.LogWarning($"웨이브 시작: {_currentWaveIndex + 1}");
        // 웨이브에 포함된 모든 적 그룹을 스폰
        foreach (EnemyGroup group in waveData.enemyGroups)
        {
            // 스포너를 사용 해서 웨이브에 할당된 적을 스폰
            enemySpawner.StartSpawning(group.enemyData, group.count, waveData.timeToNextWave);
        }
        
        // 웨이브 지속 시간 만큼 대기
        yield return new WaitForSeconds(waveData.timeToNextWave);
        
        // 이후 다음 웨이브 시작
        StartNextWave();
    }
}
