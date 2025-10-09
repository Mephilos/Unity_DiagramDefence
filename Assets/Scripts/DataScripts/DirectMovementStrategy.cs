using UnityEngine;

[CreateAssetMenu(fileName = "Direct Movement Strategy", menuName = "Game Data/Enemy Movement Strategies/Direct")]
public class DirectMovementStrategy : MovementStrategy
{
    public override Vector3 Move(Transform currentTransform, Transform targetTransform, float speed, ref float rotationTracker)
    {
        if (targetTransform == null) return Vector3.zero;

        Vector3 direction = (targetTransform.position - currentTransform.position).normalized;
        currentTransform.position += direction * speed * Time.deltaTime;

        return direction;
    }
}
