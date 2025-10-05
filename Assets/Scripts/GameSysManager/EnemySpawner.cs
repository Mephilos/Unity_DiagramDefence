using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    // public EnemyData[] enemyTypes;
    // public float spawnInterval = 1f;
    public float spawnRadius = 15f;

    [Header("타겟 설정")]
    public Transform playerTower;

    // private float _spawnTimer;
    private Coroutine _spawnCoroutine;

    // void Start()
    // {
    //     if (playerTower == null) Debug.LogError($"[{gameObject}] 적이 향할 타켓 설정 필요");
    // }

    // void Update()
    // {
    //     _spawnTimer -= Time.deltaTime;
    //     if (_spawnTimer <= 0)
    //     {
    //         SpawnEnemy();
    //         _spawnTimer = spawnInterval;
    //     }
    // }

    // void SpawnEnemy()
    // {
    //     if (enemyTypes.Length == 0) return; // 스폰 할 적의 리스트가 없으면 그냥 종료.

    //     // 적 스폰 랜덤
    //     EnemyData enemyToSpawn = enemyTypes[Random.Range(0, enemyTypes.Length)];

    //     // 스폰 위치 설정(랜덤)
    //     Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
    //     Vector3 spawnPosition = new Vector3(randomPoint.x, 1, randomPoint.y);

    //     // 적 프리팹 생성
    //     GameObject enemyObj = Instantiate(enemyToSpawn.enemyPrefab, spawnPosition, Quaternion.identity);
    //     // 적 프리팹에서 데이터 받은 후 초기화
    //     EnemyController enemyController = enemyObj.GetComponent<EnemyController>();
    //     if (enemyController != null)
    //     {
    //         enemyController.Initialize(enemyToSpawn, playerTower);
    //     }
    // }

    // 스폰 시작 함수
    public void StartSpawning(EnemyData enemyData, int count, float spawningTime)
    {   
        // 스폰이 진행중이면 코루틴을 멈춤(안전 장치)
        if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);

        // 어떤 적을 얼마나 몇 초에 걸처서 스폰 할 것인가를 받아서 스폰
       _spawnCoroutine = StartCoroutine(SpawnRoutine(enemyData, count, spawningTime));
    }


    private IEnumerator SpawnRoutine(EnemyData enemyData, int count, float spawningTime)
    {
        // 스폰 적이 0일 경우
        if (count <= 0) yield break;

        // 한 마리당 스폰 인터벌
        float spawnInterval = spawningTime / count;

        for (int i = 0; i < count; i++)
        {
            // 스폰 위치, 종류를 루프하여 스폰 시킴
            Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPoition = new Vector3(randomPoint.x, 1, randomPoint.y);

            GameObject enemyObj = Instantiate(enemyData.enemyPrefab, spawnPoition, Quaternion.identity);

            EnemyController enemyController = enemyObj.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.Initialize(enemyData, playerTower);
            }
            // 다음 스폰 시간 만큼 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
