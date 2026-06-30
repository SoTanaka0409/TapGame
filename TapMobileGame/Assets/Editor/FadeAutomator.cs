using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class FadeAutomator
{
    [MenuItem("Tools/✨全シーンにフェードを追加する✨")]
    public static void AddFadeToAllScenes()
    {
        string[] scenes = { "Assets/Scenes/Title.unity", "Assets/Scenes/Game.unity", "Assets/Scenes/Result.unity" };
        
        foreach (string scenePath in scenes)
        {
            var scene = EditorSceneManager.OpenScene(scenePath);
            
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
            }

            // 手作業で作られた古いFadeImageやFadeControllerがあれば消す
            FadeController oldFade = GameObject.FindObjectOfType<FadeController>();
            if (oldFade != null) GameObject.DestroyImmediate(oldFade.gameObject);
            GameObject manualFade = GameObject.Find("FadeImage");
            if (manualFade != null) GameObject.DestroyImmediate(manualFade);

            // FadeImageを作る
            GameObject fadeObj = new GameObject("FadeImage");
            fadeObj.transform.SetParent(canvas.transform, false);
            fadeObj.transform.SetAsLastSibling(); // 一番手前にする
            
            Image img = fadeObj.AddComponent<Image>();
            img.color = Color.black;
            img.raycastTarget = true;
            
            RectTransform rt = fadeObj.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;

            FadeController fadeCtrl = fadeObj.AddComponent<FadeController>();
            fadeCtrl.fadeImage = img;
            fadeCtrl.fadeTime = 0.5f;

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }
        
        // 元のシーン(Title)に戻る
        EditorSceneManager.OpenScene("Assets/Scenes/Title.unity");
        Debug.Log("すべてのシーンにフェード機能を追加しました！");
    }
}
