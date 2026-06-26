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

    public void RetryGame()
    {
        SceneManager.LoadScene("TitleSecne");
    }
}
