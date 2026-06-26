using UnityEngine;
using UnityEngine.UI; // UIを扱うために必要

public class ScoreManager : MonoBehaviour
{
    public int score = 0;        // 現在のスコア
    public Text scoreText;       // スコアを表示するUIテキスト

    void Start()
    {
        // 最初は0点を表示する
        UpdateScoreText();
    }

    // スコアを増やすメソッド
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    // 画面のテキストを更新するメソッド
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("ScoreTextが設定されていません！");
        }
    }
}
