using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public class RestoreGameScene
{
    [MenuItem("Tools/ゲームシーンを完全復旧する")]
    public static void Restore()
    {
        // 1. GameSceneを開く
        EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");

        // 2. カメラの復旧
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            mainCam = camObj.AddComponent<Camera>();
        }
        mainCam.transform.position = new Vector3(0, 0, -10);
        mainCam.orthographic = true;
        mainCam.orthographicSize = 5f;
        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.backgroundColor = new Color(0.2f, 0.3f, 0.4f);

        // 3. CanvasとScoreTextの復旧
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObj.AddComponent<GraphicRaycaster>();
        }

        Text scoreText = null;
        GameObject scoreTextObj = GameObject.Find("ScoreText");
        if (scoreTextObj == null)
        {
            scoreTextObj = new GameObject("ScoreText");
            scoreTextObj.transform.SetParent(canvas.transform, false);
            scoreText = scoreTextObj.AddComponent<Text>();
            
            // フォント設定（Arial）
            scoreText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            scoreText.fontSize = 100;
            scoreText.alignment = TextAnchor.UpperCenter;
            scoreText.color = Color.white;
            scoreText.text = "Score: 0";

            // 位置設定
            RectTransform rt = scoreText.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 1f);
            rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0, -100);
            rt.sizeDelta = new Vector2(800, 150);
        }
        else
        {
            scoreText = scoreTextObj.GetComponent<Text>();
        }

        // 4. ScoreManagerの復旧
        ScoreManager sm = Object.FindObjectOfType<ScoreManager>();
        if (sm == null)
        {
            GameObject smObj = new GameObject("ScoreManager");
            sm = smObj.AddComponent<ScoreManager>();
        }
        sm.scoreText = scoreText; // 紐付け

        // 5. GeneratorとGameTimerの復旧
        GameObject genObj = GameObject.Find("Generator");
        if (genObj == null)
        {
            genObj = new GameObject("Generator");
        }
        
        BallGenerator bg = genObj.GetComponent<BallGenerator>();
        if (bg == null) bg = genObj.AddComponent<BallGenerator>();

        // プレハブをジェネレーターに自動セット（あるものだけ）
        string[] prefabNames = { "ApplePrefab", "MelonPrefab", "OrangePrefab", "RottenPrefab", "BallPrefab" };
        System.Collections.Generic.List<GameObject> foundPrefabs = new System.Collections.Generic.List<GameObject>();
        foreach(var name in prefabNames)
        {
            string path = "Assets/" + name + ".prefab";
            GameObject p = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (p != null) foundPrefabs.Add(p);
        }
        bg.ballPrefabs = foundPrefabs.ToArray();

        // 制限時間タイマーの復旧
        GameTimer gt = genObj.GetComponent<GameTimer>();
        if (gt == null) gt = genObj.AddComponent<GameTimer>();

        // 6. 保存
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("【復旧完了】GameSceneを完全に再構築しました！");
    }
}
