using UnityEngine;

// 게임 상황 구분용 enum
public enum GameState
{
    Playing,
    GameOver,
    Victory
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    // 현재 게임 상태 저장 변수
    public GameState CurrentState { get; private set; }

    void Start()
    {

    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f; // 시간흐름 초기화 (안전장치)
        Debug.Log("게임시작");
    }

    public void EndGame(bool isVictory)
    {
        // 중복 호출 방지
        if (CurrentState != GameState.Playing) return;

        Time.timeScale = 0f; // 게임 시간 정지;

        if (isVictory)
        {
            CurrentState = GameState.Victory;
            UIManager.Instance.ShowVictoryWindow();
            Debug.LogWarning("승리!");
        }
        else
        {
            CurrentState = GameState.GameOver;
            UIManager.Instance.ShowGameOverWindow();
            Debug.LogWarning("배패!");
        }
    }
}
