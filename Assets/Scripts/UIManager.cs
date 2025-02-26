using System.Collections;
using System.Runtime.CompilerServices;
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
    private static Text scoreText;
    public static Text ScoreText()
    {
        if (scoreText == null) scoreText = GameObject.Find("UI/ScoreText").GetComponent<Text>();
        return scoreText;
    }
    private int score;
    public void AddScore(int pScore)
    {
        score += pScore;
        ScoreText().text = $"Score : {score}";
    }
    public void ScoreReset()
    {
        score = 0;
        ScoreText().text = $"Score : {score}";
    }
    public void SetScore(int pScore)
    {
        ScoreText().text = $"Score : {pScore}";
    }
}



public class UIManager : MonoBehaviour
{
    private Canvas mainUi;
    private Text timeText;
    private int countDown = 99;

    private void Start()
    {
        ScoreManager.Instance();
        //mainUi = GameObject.Find("UI").GetComponent<Canvas>();
        timeText = GameObject.Find("UI/TimeText").GetComponent<Text>();
        countDown = 99;
        timeText.text = $"{countDown}";

        ScoreManager.Instance().ScoreReset();

        StartCoroutine(TimeCountDown());
    }

    IEnumerator TimeCountDown()
    {
        yield return new WaitForSeconds(1);
        countDown--;
        timeText.text = $"{countDown}";
        if (countDown > 0) StartCoroutine(TimeCountDown());
    }

}
