using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class UnitActivationEvent : EventArgs
{
    public bool[] UnitActivated { get; }

    public UnitActivationEvent(bool[] unitActivated)
    {
        UnitActivated = unitActivated;
    }
}

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
    public bool[] unitActivated = { false, false, false, false };
    public int unit1SetScore;
    public int unit2SetScore;
    public int unit3SetScore;
    public int unit4SetScore;

    public event EventHandler<UnitActivationEvent> UnitActivationChanged;
    protected virtual void OnUnitActivationChanged()
    {
        UnitActivationChanged?.Invoke(this, new UnitActivationEvent(unitActivated));
    }

    public void SetScoreText()
    {
        if (scoreText == null) scoreText = GameObject.Find("UI/ScoreText").GetComponent<Text>();
        if (bestScoreText == null) bestScoreText = GameObject.Find("UI/BestScore").GetComponent<Text>();
    }
    private int score;
    public int Score
    {
        get { return score; }
    }

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

        UnitSet();
    }

    public void UnitSet()
    {
        bool[] previousActivation = (bool[])unitActivated.Clone();

        if (score >= unit1SetScore) unitActivated[0] = true;
        else unitActivated[0] = false;
        if (score >= unit2SetScore) unitActivated[1] = true;
        else unitActivated[1] = false;
        if (score >= unit3SetScore) unitActivated[2] = true;
        else unitActivated[2] = false;
        if (score >= unit4SetScore) unitActivated[3] = true;
        else unitActivated[3] = false;

        if (!Enumerable.SequenceEqual(previousActivation, unitActivated))
        {
            OnUnitActivationChanged();
        }
    }

    public void ScoreReset()
    {
        score = 0;
        UnitSet();
        bestScore = PlayerPrefs.GetInt("BestScore");
        scoreText.text = $"Score : {score}";
        bestScoreText.text = $"Best : {bestScore}";
    }
}

public class UIManager : MonoBehaviour
{
    private Text timeText;
    private Text gameOverText;
    private Image donation;
    private Image background;
    private Image backgroundWin;
    private Button retryBtn;
    private GameObject player;

    private float delay = 0.2f;

    public float fadeDuration = 1.5f;
    public float textDelay = 0.5f;
    public int countDown = 100;

    public int unit1SetScore;
    public int unit2SetScore;
    public int unit3SetScore;
    public int unit4SetScore;

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
        donation = GameObject.Find("UI/DonationImage").GetComponent<Image>();
        background = GameObject.Find("UI/Pangpang").GetComponent<Image>();
        backgroundWin = GameObject.Find("UI/PangPangWin").GetComponent<Image>();
        retryBtn = GameObject.Find("UI/RetryBtn").GetComponent<Button>();
        player = GameObject.Find("Player").gameObject;

        ScoreManager.Instance().unit1SetScore = this.unit1SetScore;
        ScoreManager.Instance().unit2SetScore = this.unit2SetScore;
        ScoreManager.Instance().unit3SetScore = this.unit3SetScore;
        ScoreManager.Instance().unit4SetScore = this.unit4SetScore;

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
        ScoreManager.Instance().difficultyLevel = 0.5f;
        countDown = initialCountDown;
        ScoreManager.Instance().ScoreReset();
        retryBtn.gameObject.SetActive(false);
        Color color = gameOverText.color;
        color.a = 0;
        gameOverText.color = color;
        color = background.color;
        color.a = 0;
        background.color = color;
        backgroundWin.color = color;
        donation.color = color;
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
                ScoreManager.Instance().difficultyLevel = 0.03f;
            }
            else if (countDown < countDownHalf)
            {
                ScoreManager.Instance().difficultyLevel = 0.2f;
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
        gameOverText.text = "";

        int typingCount = 0;
        float elapsedTime = 0f;
        string content = pWin ? "HAPPY♥PANGPANGY~" : "GAME OVER";

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

        Color finalTextColor = gameOverText.color;
        finalTextColor.a = 1;
        gameOverText.color = finalTextColor;

        while (typingCount != content.Length)
        {
            if (typingCount < content.Length)
            {
                gameOverText.text += content[typingCount].ToString();
                typingCount++;
            }
            yield return new WaitForSeconds(delay);
        }
        retryBtn.gameObject.SetActive(true);
    }

    IEnumerator FadeIn(Image pBack)
    {
        ScoreManager.Instance().gameOver = true;
        yield return new WaitForSeconds(1);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = elapsedTime / fadeDuration;
            Color bgColor = pBack.color;
            bgColor.a = alpha;
            pBack.color = bgColor;
            donation.color = bgColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Color finalBgColor = background.color;
        finalBgColor.a = 1;
        background.color = finalBgColor;
        donation.gameObject.SetActive(true);
    }
}
