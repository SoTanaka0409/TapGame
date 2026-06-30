using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public class AddTimerUI
{
    [MenuItem("Tools/タイマーの文字を自動配置する")]
    public static void AddTimer()
    {
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvasが見つかりません。先にゲームシーンを開いてください。");
            return;
        }

        // TimerTextを作成
        GameObject timerObj = GameObject.Find("TimerText");
        if (timerObj == null)
        {
            timerObj = new GameObject("TimerText");
            timerObj.transform.SetParent(canvas.transform, false);
            
            Text t = timerObj.AddComponent<Text>();
            t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            t.fontSize = 80;
            t.alignment = TextAnchor.UpperLeft;
            t.color = Color.yellow; // タイマーは目立つ黄色に
            t.text = "Time: 30.0";
            
            // 画面の左上に配置
            RectTransform rt = timerObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(50, -50);
            rt.sizeDelta = new Vector2(600, 150);
        }

        // GameTimerプログラムに紐付ける
        GameTimer gt = Object.FindObjectOfType<GameTimer>();
        if (gt != null)
        {
            gt.timerText = timerObj.GetComponent<Text>();
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("【タイマー配置完了】画面左上にタイマーの文字をセットしました！");
    }
}
