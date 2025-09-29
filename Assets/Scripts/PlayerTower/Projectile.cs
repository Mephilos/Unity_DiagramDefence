using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _damage;
    private float _speed;
    private float _lifetime;

    public void Initialize(ProjectileData data, float finalDamage, float finalSpeed)
    {
        _damage = finalDamage;
        _speed = finalSpeed;
        _lifetime = data.lifetime;

        Destroy(gameObject, _lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}
