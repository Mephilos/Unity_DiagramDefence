using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Orbit Strategy", menuName = "Game Data/Enemy Movement Strategies/Orbit Strategy")]
public class OrbitStrategy : MovementStrategy
{
    [Header("회전 반경")]
    public float orbitDistance = 5f;

    [Header("회전 속도")]
    public float orbitSpeedMultiplier = 1.0f;

    public override void Move(Transform currentTransform, Transform targetTransform, float speed, ref float rotationTracker)
    {
        if (targetTransform == null) return;

        float distanceToTarget = Vector3.Distance(currentTransform.position, targetTransform.position);

        // 타켓 거리가 회전 반경 밖일 때 접근
        if (distanceToTarget > orbitDistance)
        {
            // 방향 정규화
            Vector3 direction = (targetTransform.position - currentTransform.position).normalized;
            currentTransform.position += direction * speed * Time.deltaTime;

            rotationTracker = 0f;
        }

        else
        {
            float effectiveOrbitSpeed = speed * orbitSpeedMultiplier;
            float rotationAmount = effectiveOrbitSpeed * Time.deltaTime;
            currentTransform.RotateAround(targetTransform.position, Vector3.up, rotationAmount);
            // 회적 각도를 트래커에 더해주기
            rotationTracker += rotationAmount;
        }
    }
}
