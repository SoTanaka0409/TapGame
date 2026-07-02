using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面などから別のシーンへ遷移する処理を管理するクラス
/// </summary>
public class SceneChanger : MonoBehaviour
{
    [SerializeField, Tooltip("ボタン押下時に再生するSE")]
    private AudioClip buttonSound;

    [SerializeField, Tooltip("ボタン押下時に生成するエフェクトのプレハブ")]
    private GameObject clickEffectPrefab;

    /// <summary>
    /// メインゲームシーンへの遷移処理を開始する
    /// </summary>
    public void GoToGameScene()
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
            fade.FadeOutAndLoad("Game");
        }
        else 
        {
            SceneManager.LoadScene("Game");
        }
    }
}
