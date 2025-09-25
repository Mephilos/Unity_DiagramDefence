using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorBouns
{
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float visualScale = 1f;
    public float rotationSpeedMultiplier = 1f;
}
[CreateAssetMenu(fileName = "New Tower Data", menuName = "Game Data/Tower Data")]
public class TowerData : ScriptableObject
{
    [Header("타워 기본 정보")]
    public string towerName = "기본 4슬롯 타워";

    [Header("층별 보너스 정보")]
    public List<FloorBouns> floorBouns;
}
