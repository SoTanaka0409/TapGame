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
        GameTimer timer = FindObjectOfType<GameTimer>();
        if(timer != null )
        {
            if (timer.gameFinishFlag == true)
            {
                return;//timerg過ぎたら加点をなくす
            }
        }
        CurrentScore += amount;
        UpdateScoreText();
        StartCoroutine(BounceAnimation());
    }

    private System.Collections.IEnumerator BounceAnimation()
    {
        if (scoreText == null) yield break;

        Vector3 originalScale = Vector3.one;
        scoreText.transform.localScale = originalScale * 1.5f;
        
        float time = 0;
        float duration = 0.15f;
        while (time < duration)
        {
            time += Time.deltaTime;
            scoreText.transform.localScale = Vector3.Lerp(originalScale * 1.5f, originalScale, time / duration);
            yield return null;
        }
        scoreText.transform.localScale = originalScale;
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
