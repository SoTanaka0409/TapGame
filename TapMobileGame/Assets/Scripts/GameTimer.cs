using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 30.0f;
    public Text timerText;

    void Update()
    {
        timeLimit -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Max(0, timeLimit).ToString("F1");
        }

        if (timeLimit <= 0)
        {
            ScoreManager sm = FindObjectOfType<ScoreManager>();
            if (sm != null)
            {
                PlayerPrefs.SetInt("FinalScore", sm.score);
                PlayerPrefs.Save();
            }

            SceneManager.LoadScene("Result");
        }
    }
}
