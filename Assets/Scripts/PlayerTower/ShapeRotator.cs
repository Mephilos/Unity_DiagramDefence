using UnityEngine;

public class ShapeRotator : MonoBehaviour
{
    // 단순 회전을 위한 스크립트
    private float _rotationSpeed;

    public void Initialize(float speed)
    {
        _rotationSpeed = speed;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
