using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

class ScoreManager
{
    private static ScoreManager instance;
    public static ScoreManager Instance()
    {
        if (instance == null) instance = new ScoreManager();
        return instance;
    }
    private Text scoreText;
    private Text bestScoreText;
    public float difficultyLevel = 0.5f;
    public bool gameOver = false;
    public void SetScoreText()
    {
        if (scoreText == null) scoreText = GameObject.Find("UI/ScoreText").GetComponent<Text>();
        if (bestScoreText == null) bestScoreText = GameObject.Find("UI/BestScore").GetComponent<Text>();
    }
    private int score;
    private int bestScore;
    public void AddScore(int pScore)
    {
        score += pScore;
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }
        scoreText.text = $"Score : {score}";
        bestScoreText.text = $"Best : {bestScore}";
    }

    public void ScoreReset()
    {
        score = 0;
        bestScore = PlayerPrefs.GetInt("BestScore");
        scoreText.text = $"Score : {score}";
        bestScoreText.text = $"Best : {bestScore}";
    }
}

public class UIManager : MonoBehaviour
{
    private Text timeText;
    private Text gameOverText;
    private Image background;
    private Image backgroundWin;
    private Button retryBtn;
    private GameObject player;

    private float delay = 0.2f; // 읽는 속도

    public float fadeDuration = 1.5f; // 페이드 인 지속 시간 (초)
    public float textDelay = 0.5f;     // 배경 후 텍스트 등장 딜레이
    public int countDown = 100;

    private int countDownHalf;
    private int countDownOneThird;
    private int initialCountDown;

    private void Awake()
    {
        ScoreManager.Instance();
        ScoreManager.Instance().SetScoreText();
    }

    private void Start()
    {
        timeText = GameObject.Find("UI/TimeText").GetComponent<Text>();
        gameOverText = GameObject.Find("UI/GameOverText").GetComponent<Text>();
        background = GameObject.Find("UI/Pangpang").GetComponent<Image>();
        backgroundWin = GameObject.Find("UI/PangPangWin").GetComponent<Image>();
        retryBtn = GameObject.Find("UI/RetryBtn").GetComponent<Button>();
        player = GameObject.Find("Player").gameObject;

        retryBtn.gameObject.SetActive(false);
        timeText.text = $"{countDown}";

        countDownHalf = (int)(countDown * 0.5f);
        countDownOneThird = (int)(countDown * 0.33f);
        initialCountDown = countDown;

        ScoreManager.Instance().ScoreReset();

        StartCoroutine(TimeCountDown());
    }

    public void RetryBtnClick()
    {
        ScoreManager.Instance().gameOver = false;
        countDown = initialCountDown;
        #region Comment
        //// Player 오브젝트 재생성
        //if (GameObject.FindGameObjectWithTag("Player") == null)
        //{
        //    GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        //    if (playerPrefab != null)
        //    {
        //        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //    }
        //    else
        //    {
        //        Debug.LogError("Player 프리팹을 찾을 수 없습니다.");
        //    }
        //}
        #endregion

        if (player.activeSelf == false)
        {
            player.SetActive(true);
            player.transform.position = new Vector3(-6.9f, 1f, 0f);
        }

        // 게임 상태 초기화
        ResetGameState();

        StartCoroutine(TimeCountDown());
    }

    private void ResetGameState()
    {
        ScoreManager.Instance().ScoreReset();
        retryBtn.gameObject.SetActive(false);
        Color color = gameOverText.color;
        color.a = 0;
        gameOverText.color = color;
        color = background.color;
        color.a = 0;
        background.color = color;
        backgroundWin.color = color;
    }



    IEnumerator TimeCountDown()
    {
        while (countDown > 0)
        {
            if (ScoreManager.Instance().gameOver) break;
            yield return new WaitForSeconds(1);
            countDown--;
            timeText.text = $"{countDown}";
            if (countDown < countDownOneThird)
            {
                ScoreManager.Instance().difficultyLevel = 0.05f;
            }
            else if (countDown < countDownHalf)
            {
                ScoreManager.Instance().difficultyLevel = 0.3f;
            }
            else
            {
                ScoreManager.Instance().difficultyLevel = 0.5f;
            }
        }

        if (countDown > 0)
        {
            StartCoroutine(FadeIn(background));
            StartCoroutine(TypingGameOver(false));
        }
        else
        {
            StartCoroutine(FadeIn(backgroundWin));
            StartCoroutine(TypingGameOver(true));
        }
    }

    IEnumerator TypingGameOver(bool pWin)
    {
        yield return new WaitForSeconds(1);
        gameOverText.text = ""; // 현재 화면 메세지 비움

        int typingCount = 0; // 타이핑 카운트 0 초기화
        float elapsedTime = 0f;
        string content = pWin ? "HAPYP PANGPANGY~♥" : "GAME OBER";

        // 텍스트 페이드 인
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = elapsedTime / fadeDuration;
            Color textColor = gameOverText.color;
            textColor.a = alpha;
            textColor.r = pWin ? 0 : 255;
            textColor.g = 0;
            textColor.b = pWin ? 255 : 0;
            gameOverText.color = textColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 텍스트를 완전히 불투명하게 설정
        Color finalTextColor = gameOverText.color;
        finalTextColor.a = 1;
        gameOverText.color = finalTextColor;

        // 현재 카운트가 컨텐츠의 길이와 다르다면 
        while (typingCount != content.Length)
        {
            if (typingCount < content.Length)
            {
                gameOverText.text += content[typingCount].ToString();
                // 현재 카운트에 해당하는 단어 하나를 메세지 텍스트 UI에 전달
                typingCount++;
                // 카운트를 1 증가
            }
            yield return new WaitForSeconds(delay);
            // 현재의 딜레이만큼 대기
        }

        retryBtn.gameObject.SetActive(true);

    }

    IEnumerator FadeIn(Image pBack)
    {
        ScoreManager.Instance().gameOver = true;
        yield return new WaitForSeconds(1);
        float elapsedTime = 0f;

        // 배경 페이드 인
        while (elapsedTime < fadeDuration)
        {
            float alpha = elapsedTime / fadeDuration;
            Color bgColor = pBack.color;
            bgColor.a = alpha;
            pBack.color = bgColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 배경을 완전히 불투명하게 설정
        Color finalBgColor = background.color;
        finalBgColor.a = 1;
        background.color = finalBgColor;
    }


}
