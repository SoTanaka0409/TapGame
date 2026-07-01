using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public Text finalScoreText;

    void Start()
    {
        if (finalScoreText != null)
        {
            int score = PlayerPrefs.GetInt("FinalScore", 0);
            finalScoreText.text = "最終スコア:\n" + score + " 点";
        }
    }

    // 鳴らしたいボタン音（SE）を入れる枠
    public AudioClip buttonSound;

    public void RetryGame()
    {
        // 音が設定されていれば、カメラの位置（大音量）で一瞬だけ鳴らす
        if (buttonSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonSound, Camera.main.transform.position);
        }

        FadeController fade = FindObjectOfType<FadeController>();
        if (fade != null) fade.FadeOutAndLoad("Title");
        else SceneManager.LoadScene("Title");
    }
}
