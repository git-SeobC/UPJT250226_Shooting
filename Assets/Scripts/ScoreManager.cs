using UnityEngine;
using UnityEngine.UI;

public class ScoreManagers : MonoBehaviour
{
    #region Singleton
    public static ScoreManagers Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("Best Score");
        currentScoreUI.text = $"Current : {currentScore}";
    }

    // Inspector
    public Text currentScoreUI;
    public Text bestScoreUI;
    // Inner
    private int currentScore = 0;
    private int bestScore;

    // 현재 점수에 대한 프로퍼티 설계
    // 값에 대한 접근과 설정을 변수처럼 진행할 수 있음
    public int Score
    {
        get
        {
            return currentScore;
        }
        set
        {
            // 1. 전달받은 값이 현재의 점수로 설정
            currentScore = value;
            // 2. UI에 대항 값이 적용
            currentScoreUI.text = $"현재 점수 : {currentScore}";

            // 현재의 점수가 최고 점수를 넘었다면
            if (currentScore > bestScore)
            {
                // 그 점수가 최고 점수로 설정
                bestScore = currentScore;
                // UI 갱신
                bestScoreUI.text = $"최고 점수 : {bestScore}";
                // 내부 데이터에도 그 수치 적용
                PlayerPrefs.SetInt("Best Score", bestScore);
            }
        }
    }

}
