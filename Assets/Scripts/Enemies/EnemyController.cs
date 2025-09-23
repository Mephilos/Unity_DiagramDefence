using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyData _enemyData;
    private float _currentHp;
    private Transform _target;

    public void Initialize(EnemyData data, Transform target)
    {
        _enemyData = data;
        _target = target;
        _currentHp = _enemyData.maxHp;
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_target == null) return;

        Vector3 direction = (_target.position - transform.position).normalized;
        transform.position += direction * _enemyData.moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(float damageAmount)
    {
        _currentHp -= damageAmount;
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTowerController player = other.GetComponent<PlayerTowerController>();
            if (player != null)
            {
                player.TakeDamage(_enemyData.damage);
            }
            Destroy(gameObject);
        }
    }
}
