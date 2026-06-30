using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 30.0f;
    public Text timerText;
    private bool isEnded = false;

    void Update()
    {
        timeLimit -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Max(0, timeLimit).ToString("F1");

            // 残り10秒以下になったら躍動させる
            if (timeLimit <= 10.0f)
            {
                // 文字を警告の赤色にする
                timerText.color = Color.red;

                // Mathf.Sin() を使って、波のように数字を上下させる
                float wave = Mathf.Abs(Mathf.Sin(Time.time * 10f));
                float scale = 1.0f + (wave * 0.5f);
                
                // 実際に文字の大きさを変更する
                timerText.transform.localScale = new Vector3(scale, scale, 1.0f);
            }
        }

        if (timeLimit <= 0 && !isEnded)
        {
            isEnded = true; // 1回だけ実行するようにフラグを立てる
            ScoreManager sm = FindObjectOfType<ScoreManager>();
            if (sm != null)
            {
                PlayerPrefs.SetInt("FinalScore", sm.score);
                PlayerPrefs.Save();
            }

            FadeController fade = FindObjectOfType<FadeController>();
            if (fade != null) fade.FadeOutAndLoad("Result");
            else SceneManager.LoadScene("Result");
        }
    }
}
