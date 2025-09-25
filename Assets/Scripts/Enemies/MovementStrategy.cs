using UnityEngine;

// 적들의 이동을 패턴을 전략 패턴으로 구성
public abstract class MovementStrategy : ScriptableObject
{
    public abstract void Move(Transform currentTransform, Transform targetTransform, float speed, ref float rotationTracker);
}
