using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    public EnemyData[] enemyTypes;
    public float spawnInterval = 1f;
    public float spawnRadius = 15f;

    [Header("타겟 설정")]
    public Transform playerTower;

    private float _spawnTimer;

    void Start()
    {
        if (playerTower == null) Debug.LogError($"[{gameObject}] 적이 향할 타켓 설정 필요");
    }

    void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            SpawnEnemy();
            _spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (enemyTypes.Length == 0) return; // 스폰 할 적의 리스트가 없으면 그냥 종료.

        // 적 스폰 랜덤
        EnemyData enemyToSpawn = enemyTypes[Random.Range(0, enemyTypes.Length)];

        // 스폰 위치 설정(랜덤)
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPoint.x, 1, randomPoint.y);

        // 적 프리팹 생성
        GameObject enemyObj = Instantiate(enemyToSpawn.enemyPrefab, spawnPosition, Quaternion.identity);
        // 적 프리팹에서 데이터 받은 후 초기화
        EnemyController enemyController = enemyObj.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.Initialize(enemyToSpawn, playerTower);
        }
    }
}
