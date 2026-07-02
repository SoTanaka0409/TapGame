using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// リザルト画面における最終スコア表示とリトライ処理を管理するクラス
/// </summary>
public class ResultManager : MonoBehaviour
{
    [SerializeField, Tooltip("最終スコアを表示するUIテキスト")]
    private Text finalScoreText;

    [SerializeField, Tooltip("リトライボタン押下時に再生するSE")]
    private AudioClip buttonSound;

    [SerializeField, Tooltip("リトライボタン押下時に生成するエフェクトのプレハブ")]
    private GameObject clickEffectPrefab;

    private void Start()
    {
        DisplayFinalScore();
    }

    /// <summary>
    /// PlayerPrefsから保存されたスコアを読み込み、UIに表示する
    /// </summary>
    private void DisplayFinalScore()
    {
        if (finalScoreText != null)
        {
            int score = PlayerPrefs.GetInt("FinalScore", 0);
            finalScoreText.text = $"最終スコア:\n{score} 点";
        }
    }

    /// <summary>
    /// タイトル画面へ戻るリトライ処理を開始する
    /// </summary>
    public void RetryGame()
    {
        if (buttonSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonSound, Camera.main.transform.position);
        }

        if (clickEffectPrefab != null)
        {
            // マウス（またはタップ）された位置にエフェクトを生成
            Vector3 clickPos = Input.mousePosition;
            clickPos.z = 10f; // カメラからの距離
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(clickPos);
            Instantiate(clickEffectPrefab, worldPos, Quaternion.identity);
        }

        FadeController fade = FindObjectOfType<FadeController>();
        if (fade != null) 
        {
            fade.FadeOutAndLoad("Title");
        }
        else 
        {
            SceneManager.LoadScene("Title");
        }
    }
}
