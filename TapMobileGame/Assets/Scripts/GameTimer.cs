using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームの制限時間を管理し、時間切れ時のリザルト画面遷移を行うクラス
/// </summary>
public class GameTimer : MonoBehaviour
{
    [SerializeField, Tooltip("ゲームの制限時間（秒）")]
    private float timeLimit = 30.0f;

    [SerializeField, Tooltip("残り時間を表示するUIテキスト")]
    private Text timerText;

    [SerializeField, Tooltip("タイマーが赤く点滅し始める残り時間（秒）")]
    private float warningTimeThreshold = 10.0f;

    public bool IsGameFinished { get; private set; } = false;

    /// <summary>
    /// 現在の残り時間を外部から取得するためのプロパティ
    /// </summary>
    public float CurrentTimeLimit => timeLimit;

    private bool isEnded = false;

    private void Update()
    {
        if (isEnded) return;

        timeLimit -= Time.deltaTime;
        UpdateTimerUI();

        if (timeLimit <= 0)
        {
            EndGame();
        }
    }

    /// <summary>
    /// UIテキストの更新および、残り時間が少ない場合の演出（警告色・振動）を処理する
    /// </summary>
    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        timerText.text = $"Time: {Mathf.Max(0, timeLimit):F1}";

        if (timeLimit <= warningTimeThreshold)
        {
            timerText.color = Color.red;

            // 波打つようなスケールアニメーションでプレイヤーに切迫感を与える
            float wave = Mathf.Abs(Mathf.Sin(Time.time * 10f));
            float scale = 1.0f + (wave * 0.5f);
            timerText.transform.localScale = new Vector3(scale, scale, 1.0f);
        }
    }

    /// <summary>
    /// 制限時間到達時の終了処理（スコア保存とリザルトシーンへの遷移）を行う
    /// </summary>
    private void EndGame()
    {
        isEnded = true;
        IsGameFinished = true;
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            PlayerPrefs.SetInt("FinalScore", scoreManager.CurrentScore);
            PlayerPrefs.Save();
        }

        FadeController fadeController = FindObjectOfType<FadeController>();
        if (fadeController != null) 
        {
            fadeController.FadeOutAndLoad("Result");
        }
        else 
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Result");
        }
    }
}
