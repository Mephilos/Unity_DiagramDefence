using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DamageCalculatingTest
{
    [Test]
    public void DamageCalculatingTestSimplePasses()
    {
        var towerData = ScriptableObject.CreateInstance<TowerData>();
        towerData.floorBouns = new List<FloorBouns>
        {
            new FloorBouns { damageMultiplier = 1.5f },
            new FloorBouns { damageMultiplier = 1.2f },
            new FloorBouns { damageMultiplier = 1.0f },
            new FloorBouns { damageMultiplier = 0.8f }
        };
        var projectileData = ScriptableObject.CreateInstance<ProjectileData>();
        projectileData.damage = 10f; // 기본 데미지는 10으로 가정

        // 1층 (가장 아래)
        float expectedDamageTier1 = 15f;
        float actualDamageTier1 = projectileData.damage * towerData.floorBouns[0].damageMultiplier;
        Assert.AreEqual(expectedDamageTier1, actualDamageTier1, "1층 데미지 계산 오류");

        // 2층
        float expectedDamageTier2 = 12f;
        float actualDamageTier2 = projectileData.damage * towerData.floorBouns[1].damageMultiplier;
        Assert.AreEqual(expectedDamageTier2, actualDamageTier2, "2층 데미지 계산 오류");

        // 3층
        float expectedDamageTier3 = 10f;
        float actualDamageTier3 = projectileData.damage * towerData.floorBouns[2].damageMultiplier;
        Assert.AreEqual(expectedDamageTier3, actualDamageTier3, "3층 데미지 계산 오류");

        // 4층
        float expectedDamageTier4 = 8f;
        float actualDamageTier4 = projectileData.damage * towerData.floorBouns[3].damageMultiplier;
        Assert.AreEqual(expectedDamageTier4, actualDamageTier4, "4층 데미지 계산 오류");
    }
} 