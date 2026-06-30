using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIFixer
{
    [MenuItem("Tools/UIをスマホ画面ピッタリに直す")]
    public static void FixUI()
    {
        // 1. Canvasの拡大縮小設定を「画面サイズに合わせる（スマホ基準）」に直す
        CanvasScaler scaler = Object.FindObjectOfType<CanvasScaler>();
        if (scaler != null)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920); // 一般的なスマホの縦画面
            scaler.matchWidthOrHeight = 0.5f;
        }

        // 2. ボタン（タイトル画面用など）を画面のど真ん中に大きく配置
        Button btn = Object.FindObjectOfType<Button>();
        if (btn != null)
        {
            RectTransform rt = btn.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f); // 中央基準
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero; // ど真ん中
            rt.sizeDelta = new Vector2(600, 200); // 大きくする

            Text txt = btn.GetComponentInChildren<Text>();
            if (txt != null)
            {
                txt.fontSize = 80;
                if (txt.text == "Button") txt.text = "START";
            }
        }

        // 3. スコアテキスト（ゲーム画面用）を画面上部に大きく配置
        ScoreManager sm = Object.FindObjectOfType<ScoreManager>();
        if (sm != null && sm.scoreText != null)
        {
            RectTransform rt = sm.scoreText.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 1f); // 上部基準
            rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0, -100); // 上から少し下げる
            rt.sizeDelta = new Vector2(800, 150); // 大きくする
            
            sm.scoreText.fontSize = 100;
            sm.scoreText.alignment = TextAnchor.UpperCenter;
            sm.scoreText.color = Color.white; // 見やすいように白
        }

        // 変更を保存
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        Debug.Log("【UI自動調整】文字の大きさや位置をスマホ画面にピッタリ合うように修正しました！");
    }
}
