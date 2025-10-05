using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CreateAssetMenu(fileName = "New Stage Data", menuName = "Game Data/Stage Data")]
public class StageData : ScriptableObject
{
    [Tooltip("스테이지에서 순서대로 나올 웨이브 구성")]
    public List<WaveData> waves;
}
