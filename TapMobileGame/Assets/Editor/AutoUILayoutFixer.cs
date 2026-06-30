using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[InitializeOnLoad]
public class AutoUILayoutFixer
{
    static AutoUILayoutFixer()
    {
        // コンパイル直後に1回だけ実行する
        EditorApplication.delayCall += RunOnce;
    }

    static void RunOnce()
    {
        // 既に実行済みなら何もしない
        string prefsKey = "UILayoutFixed_ScoreRight";
        if (EditorPrefs.GetBool(prefsKey, false)) return;
        EditorPrefs.SetBool(prefsKey, true);

        // Gameシーンを開く
        var scene = EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        
        GameObject timerObj = GameObject.Find("TimerText");
        GameObject scoreObj = GameObject.Find("ScoreText");

        if (timerObj != null && scoreObj != null)
        {
            // 時間（Timer）を左上に配置
            RectTransform trt = timerObj.GetComponent<RectTransform>();
            trt.anchorMin = new Vector2(0, 1);
            trt.anchorMax = new Vector2(0, 1);
            trt.pivot = new Vector2(0, 1);
            trt.anchoredPosition = new Vector2(50, -50); // 左から50、上から50
            Text tText = trt.GetComponent<Text>();
            if (tText != null) tText.alignment = TextAnchor.UpperLeft;

            // スコア（Score）を右上に配置（時間の右側）
            RectTransform srt = scoreObj.GetComponent<RectTransform>();
            srt.anchorMin = new Vector2(1, 1);
            srt.anchorMax = new Vector2(1, 1);
            srt.pivot = new Vector2(1, 1);
            srt.anchoredPosition = new Vector2(-50, -50); // 右から50、上から50
            Text sText = srt.GetComponent<Text>();
            if (sText != null) sText.alignment = TextAnchor.UpperRight;

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("UIを「左上に時間、右上にスコア」のスマホ標準レイアウトに自動修正しました！");
        }
    }
}
