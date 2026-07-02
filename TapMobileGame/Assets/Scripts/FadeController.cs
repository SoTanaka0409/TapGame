using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移時のフェードイン・フェードアウト演出を管理するクラス
/// </summary>
public class FadeController : MonoBehaviour
{
    [SerializeField, Tooltip("フェード演出用の画面全体を覆う黒いImage")]
    private Image fadeImage;

    [SerializeField, Tooltip("フェードにかかる時間（秒）")]
    private float fadeDuration = 1.0f;

    //fadeのon.off
    private bool isFading = false;

    private void Start()
    {
        StartCoroutine(FadeInRoutine());
    }

    /// <summary>
    /// シーン開始時のフェードイン（黒から透明）アニメーションを実行する
    /// </summary>
    private IEnumerator FadeInRoutine()
    {
        if (fadeImage == null) yield break;

        fadeImage.raycastTarget = true; 
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = 1.0f - (time / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.raycastTarget = false;
    }

    /// <summary>
    /// フェードアウト演出を行い、完了後に指定されたシーンへ遷移する
    /// </summary>
    /// <param name="nextSceneName">遷移先のシーン名</param>
    public void FadeOutAndLoad(string nextSceneName)
    {
        if (isFading) return; 
        isFading = true;
        StartCoroutine(FadeOutRoutine(nextSceneName));
    }

    /// <summary>
    /// フェードアウト（透明から黒）アニメーションを実行し、シーンをロードする
    /// </summary>
    private IEnumerator FadeOutRoutine(string sceneName)
    {
        if (fadeImage != null)
        {
            fadeImage.raycastTarget = true;
            float time = 0;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float alpha = time / fadeDuration;
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            fadeImage.color = new Color(0, 0, 0, 1);
        }

        SceneManager.LoadScene(sceneName);
    }
}