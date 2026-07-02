using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイ中のスコア加算処理およびUI表示の更新を管理するクラス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField, Tooltip("スコアを表示するUIテキスト")]
    private Text scoreText;

    /// <summary>
    /// 現在のスコア値（外部からは取得のみ可能）
    /// </summary>
    public int CurrentScore { get; private set; } = 0;

    private void Start()
    {
        UpdateScoreText();
    }

    /// <summary>
    /// ターゲットをタップした際にスコアを加算する
    /// </summary>
    /// <param name="amount">加算する点数</param>
    public void AddScore(int amount)
    {
        CurrentScore += amount;
        UpdateScoreText();
    }

    /// <summary>
    /// 内部のスコア値を元にUIテキストの表示を更新する
    /// </summary>
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {CurrentScore}";
        }
        else
        {
            Debug.LogWarning("ScoreManager: ScoreTextがアタッチされていません。");
        }
    }
}
