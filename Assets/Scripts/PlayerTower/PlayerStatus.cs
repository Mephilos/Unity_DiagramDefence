using UnityEngine;
using System;

public class PlayerStatus : MonoBehaviour
{
    [Header("플레이어 HP")]
    public float maxHp = 100f;

    [Header("레벨, 경험치")]
    [SerializeField] private int level = 1;
    [SerializeField] private float currentExperience = 0f;
    [SerializeField] private float requiredExperience = 100f;

    public float CurrentHp { get; private set; }
    public int Level => level;

    // 이벤트 등록
    public event Action OnLevelUp;

    void Awake()
    {
        CurrentHp = maxHp;
    }

    public void GainExperience(float amount)
    {
        currentExperience += amount;
        Debug.Log($"[{gameObject}] 경험치 {amount} 획득 / 현재 경험치 {currentExperience} / {requiredExperience}");

        while (currentExperience >= requiredExperience)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentExperience -= requiredExperience;
        level++;
        requiredExperience *= 1.2f; // 요구 경험치 증가 로직
        Debug.LogWarning($"레벨업, 현재 레벨: {level}");
        // 이벤트 호출
        OnLevelUp?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;

        if (CurrentHp <= 0) Die();
    }
    
    void Die()
    {
        GameManager.Instance.EndGame(false);
    }
}
