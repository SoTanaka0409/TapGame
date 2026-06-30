using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;       // 手順1で作った黒いImageを入れる
    public float fadeTime = 1.0f; // フェードにかかる秒数

    void Start()
    {
        // シーンが始まったら、自動で「フェードイン（黒から透明へ）」する
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fadeImage.raycastTarget = true; // 透明になるまでタップをブロック

        float time = 0;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            float alpha = 1.0f - (time / fadeTime); // 1から0へ減らす
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // 完全に透明になったら、裏のボタンを押せるようにブロック解除
        fadeImage.raycastTarget = false;
    }

    // 次のシーンへ行きたい時にこれを呼ぶ
    private bool isFading = false;

    public void FadeOutAndLoad(string nextSceneName)
    {
        if (isFading) return; // すでにフェード中なら無視する
        isFading = true;
        StartCoroutine(FadeOut(nextSceneName));
    }

    IEnumerator FadeOut(string sceneName)
    {
        fadeImage.raycastTarget = true; // タップをブロック
        float time = 0;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            float alpha = time / fadeTime; // 0から1へ増やす
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // 完全に真っ黒になったら、次のシーンを読み込む
        SceneManager.LoadScene(sceneName);
    }
}