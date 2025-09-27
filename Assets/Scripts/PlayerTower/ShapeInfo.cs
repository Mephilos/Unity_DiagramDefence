using System.Collections.Generic;
using UnityEngine;

public class ShapeInfo : MonoBehaviour
{
    // 도형에서 발사될 투사체 위치
    [Tooltip("도형의 투사체 발사 위치")]
    public List<Transform> firePoints;
}
