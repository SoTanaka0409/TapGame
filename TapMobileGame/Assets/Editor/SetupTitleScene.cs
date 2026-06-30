using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Events;
using System.IO;

public class SetupTitleScene
{
    [MenuItem("Tools/タイトル画面のボタンを自動設定する")]
    public static void Setup()
    {
        // タイトルシーンを強制的に開く
        string scenePath = "Assets/Scenes/TitleSecne.unity";
        if (File.Exists(scenePath))
        {
            EditorSceneManager.OpenScene(scenePath);
        }
        else
        {
            Debug.LogError("TitleSecne.unity が見つかりません。");
            return;
        }

        // Canvasを探す
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // EventSystemを探す（ボタンのクリック判定に必須）
        UnityEngine.EventSystems.EventSystem es = Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (es == null)
        {
            GameObject esObj = new GameObject("EventSystem");
            esObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Buttonを探す
        Button btn = Object.FindObjectOfType<Button>();
        if (btn == null)
        {
            // なければデフォルトのボタンを生成
            GameObject btnObj = DefaultControls.CreateButton(new DefaultControls.Resources());
            btnObj.transform.SetParent(canvas.transform, false);
            btn = btnObj.GetComponent<Button>();
        }

        // SceneChangerスクリプトをアタッチ
        SceneChanger changer = Object.FindObjectOfType<SceneChanger>();
        if (changer == null)
        {
            changer = canvas.gameObject.AddComponent<SceneChanger>();
        }

        // ボタンのクリックイベントをすべてクリアしてから再登録
        var clickEvent = btn.onClick;
        for (int i = clickEvent.GetPersistentEventCount() - 1; i >= 0; i--)
        {
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(clickEvent, i);
        }

        // GoToGameScene メソッドをボタンのイベントに登録
        UnityAction action = new UnityAction(changer.GoToGameScene);
        UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(clickEvent, action);

        // シーンを保存
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("【設定完了】タイトル画面のボタンからゲーム画面へ移動できるように設定しました！");
    }
}
