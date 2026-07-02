using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面などから別のシーンへ遷移する処理を管理するクラス
/// </summary>
public class SceneChanger : MonoBehaviour
{
    [SerializeField, Tooltip("ボタン押下時に再生するSE")]
    private AudioClip buttonSound;

    /// <summary>
    /// メインゲームシーンへの遷移処理を開始する
    /// </summary>
    public void GoToGameScene()
    {
        if (buttonSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonSound, Camera.main.transform.position);
        }

        FadeController fade = FindObjectOfType<FadeController>();
        if (fade != null) 
        {
            fade.FadeOutAndLoad("Game");
        }
        else 
        {
            SceneManager.LoadScene("Game");
        }
    }
}
