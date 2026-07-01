using UnityEngine;
using UnityEngine.SceneManagement; // シーン移動に必須

public class SceneChanger : MonoBehaviour
{
    // 鳴らしたいボタン音（SE）を入れる枠
    public AudioClip buttonSound;

    // ボタンが押されたときに呼ばれるメソッド
    public void GoToGameScene()
    {
        // 音が設定されていれば、カメラの位置（大音量）で一瞬だけ鳴らす
        if (buttonSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonSound, Camera.main.transform.position);
        }

        FadeController fade = FindObjectOfType<FadeController>();
        if (fade != null) fade.FadeOutAndLoad("Game");
        else SceneManager.LoadScene("Game");
    }
}
