using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Events;

public class FinalAutomator
{
    [MenuItem("Tools/★★★ゲーム全体を完全完成させる★★★")]
    public static void SetupEverything()
    {
        // ------------------------
        // 1. ResultScene の構築
        // ------------------------
        EditorSceneManager.OpenScene("Assets/Scenes/ResultScene.unity");
        SetupCanvas(out Canvas resCanvas);
        RemoveBackground();
        SetCameraColor();
        
        Text resText = CreateText(resCanvas, "FinalScoreText", "最終スコア:\n0 点", 100, new Vector2(0, 200));
        Button retryBtn = CreateButton(resCanvas, "RetryButton", "もう一度遊ぶ", new Vector2(0, -100));
        
        ResultManager rm = Object.FindObjectOfType<ResultManager>();
        if (rm == null) rm = resCanvas.gameObject.AddComponent<ResultManager>();
        rm.finalScoreText = resText;
        
        BindButtonClick(retryBtn, rm, "RetryGame");
        SaveCurrentScene();

        // ------------------------
        // 2. TitleScene の構築
        // ------------------------
        EditorSceneManager.OpenScene("Assets/Scenes/TitleSecne.unity");
        SetupCanvas(out Canvas titleCanvas);
        RemoveBackground();
        SetCameraColor();

        Text titleText = CreateText(titleCanvas, "TitleText", "サークル\nキャッチ！", 150, new Vector2(0, 250));
        Button startBtn = CreateButton(titleCanvas, "StartButton", "START", new Vector2(0, -200));

        SceneChanger sc = Object.FindObjectOfType<SceneChanger>();
        if (sc == null) sc = titleCanvas.gameObject.AddComponent<SceneChanger>();
        
        BindButtonClick(startBtn, sc, "GoToGameScene");
        SaveCurrentScene();

        // ------------------------
        // 3. GameScene の構築
        // ------------------------
        EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");
        SetupCanvas(out Canvas gameCanvas);
        RemoveBackground();
        SetCameraColor();

        Text scoreText = CreateText(gameCanvas, "ScoreText", "Score: 0", 100, new Vector2(0, -100));
        RectTransform srt = scoreText.GetComponent<RectTransform>();
        srt.anchorMin = new Vector2(0.5f, 1); srt.anchorMax = new Vector2(0.5f, 1); srt.pivot = new Vector2(0.5f, 1); srt.anchoredPosition = new Vector2(0, -100);

        Text timerText = CreateText(gameCanvas, "TimerText", "Time: 30.0", 80, new Vector2(50, -50));
        RectTransform trt = timerText.GetComponent<RectTransform>();
        trt.anchorMin = new Vector2(0, 1); trt.anchorMax = new Vector2(0, 1); trt.pivot = new Vector2(0, 1); trt.anchoredPosition = new Vector2(50, -50);
        timerText.alignment = TextAnchor.UpperLeft;
        timerText.color = Color.yellow;

        ScoreManager sm = Object.FindObjectOfType<ScoreManager>();
        if (sm == null) sm = new GameObject("ScoreManager").AddComponent<ScoreManager>();
        sm.scoreText = scoreText;

        GameObject genObj = GameObject.Find("Generator");
        if (genObj == null) genObj = new GameObject("Generator");
        BallGenerator bg = genObj.GetComponent<BallGenerator>();
        if (bg == null) bg = genObj.AddComponent<BallGenerator>();

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        System.Collections.Generic.List<GameObject> pList = new System.Collections.Generic.List<GameObject>();
        foreach(var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject p = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (p != null)
            {
                SpriteRenderer sr = p.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = 5;
                    if (p.GetComponent<Collider2D>() == null) p.AddComponent<CircleCollider2D>();
                    if (p.GetComponent<Rigidbody2D>() == null) p.AddComponent<Rigidbody2D>();
                    if (p.GetComponent<DestroyOnClick>() == null) p.AddComponent<DestroyOnClick>();
                    pList.Add(p);
                }
            }
        }
        bg.ballPrefabs = pList.ToArray();

        GameTimer gt = genObj.GetComponent<GameTimer>();
        if (gt == null) gt = genObj.AddComponent<GameTimer>();
        gt.timerText = timerText;

        SaveCurrentScene();

        Debug.Log("【再設定完了】フルーツ要素をすべて削除し、シンプルなサークルキャッチゲームとして再構築しました！");
    }

    private static void SetupCanvas(out Canvas canvas)
    {
        canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject cObj = new GameObject("Canvas");
            canvas = cObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler cs = cObj.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1080, 1920);
            cs.matchWidthOrHeight = 0.5f;
            cObj.AddComponent<GraphicRaycaster>();
        }
        
        if (Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject esObj = new GameObject("EventSystem");
            esObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    private static void RemoveBackground()
    {
        GameObject bgObj = GameObject.Find("Background");
        if (bgObj != null) Object.DestroyImmediate(bgObj);
    }
    
    private static void SetCameraColor()
    {
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            GameObject camObj = GameObject.Find("Main Camera");
            if (camObj == null) camObj = new GameObject("Main Camera");
            mainCam = camObj.GetComponent<Camera>();
            if (mainCam == null) mainCam = camObj.AddComponent<Camera>();
        }
        mainCam.tag = "MainCamera";
        mainCam.transform.position = new Vector3(0, 0, -10);
        mainCam.orthographic = true;
        mainCam.orthographicSize = 5f;
        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.backgroundColor = new Color(0.15f, 0.2f, 0.3f); // 綺麗なダークブルーの背景
    }

    private static Text CreateText(Canvas canvas, string name, string content, int size, Vector2 pos)
    {
        GameObject tObj = GameObject.Find(name);
        if (tObj == null) tObj = new GameObject(name);
        tObj.transform.SetParent(canvas.transform, false);
        Text t = tObj.GetComponent<Text>();
        if (t == null) t = tObj.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = size;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;
        t.text = content;
        
        RectTransform rt = t.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(800, 300);
        return t;
    }

    private static Button CreateButton(Canvas canvas, string name, string textContent, Vector2 pos)
    {
        GameObject bObj = GameObject.Find(name);
        if (bObj == null)
        {
            bObj = DefaultControls.CreateButton(new DefaultControls.Resources());
            bObj.name = name;
            bObj.transform.SetParent(canvas.transform, false);
        }
        RectTransform rt = bObj.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(500, 150);
        
        Text t = bObj.GetComponentInChildren<Text>();
        if (t != null)
        {
            t.text = textContent;
            t.fontSize = 60;
        }
        return bObj.GetComponent<Button>();
    }

    private static void BindButtonClick(Button btn, MonoBehaviour target, string methodName)
    {
        var clickEvent = btn.onClick;
        for (int i = clickEvent.GetPersistentEventCount() - 1; i >= 0; i--)
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(clickEvent, i);
            
        UnityAction action = (UnityAction)System.Delegate.CreateDelegate(typeof(UnityAction), target, methodName);
        UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(clickEvent, action);
    }

    private static void SaveCurrentScene()
    {
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }
}
