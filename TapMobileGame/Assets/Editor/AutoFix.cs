using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class AutoFix
{
    [MenuItem("Tools/全部なおす (自動修正)")]
    public static void FixEverything()
    {
        // 1. 現在開いているシーンを取得（Game.unity か GameScene.unity かに関わらず、今見ている画面を直す）
        var activeScene = EditorSceneManager.GetActiveScene();

        // 2. カメラを修復・設定
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            GameObject camObj = GameObject.Find("Main Camera");
            if (camObj == null) camObj = new GameObject("Main Camera");
            mainCam = camObj.GetComponent<Camera>();
            if (mainCam == null) mainCam = camObj.AddComponent<Camera>();
        }
        mainCam.gameObject.tag = "MainCamera";
        mainCam.transform.position = new Vector3(0, 0, -10);
        mainCam.orthographic = true;
        mainCam.orthographicSize = 5f;
        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.backgroundColor = new Color(0.2f, 0.3f, 0.4f); // 少しおしゃれな青系背景

        // 3. 円の画像を作成 (すでになければ)
        string texPath = "Assets/CircleTex.png";
        if (!File.Exists(texPath))
        {
            Texture2D tex = new Texture2D(128, 128);
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(64, 64));
                    tex.SetPixel(x, y, dist <= 60 ? Color.white : Color.clear);
                }
            }
            tex.Apply();
            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(texPath, bytes);
            AssetDatabase.Refresh();
        }

        // 画像をスプライトとして設定
        TextureImporter importer = AssetImporter.GetAtPath(texPath) as TextureImporter;
        if (importer != null && importer.textureType != TextureImporterType.Sprite)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.SaveAndReimport();
        }

        Sprite circleSprite = AssetDatabase.LoadAssetAtPath<Sprite>(texPath);

        // 4. 円のプレハブを作成
        string prefabPath = "Assets/BallPrefab.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null)
        {
            GameObject tempBall = new GameObject("Ball");
            SpriteRenderer sr = tempBall.AddComponent<SpriteRenderer>();
            sr.sprite = circleSprite;
            sr.color = new Color(1f, 0.8f, 0.2f); // 綺麗な黄色

            tempBall.AddComponent<CircleCollider2D>();
            tempBall.AddComponent<DestroyOnClick>();

            prefab = PrefabUtility.SaveAsPrefabAsset(tempBall, prefabPath);
            GameObject.DestroyImmediate(tempBall);
        }

        // 5. ジェネレーターをシーンに配置
        GameObject genObj = GameObject.Find("Generator");
        if (genObj == null)
        {
            genObj = new GameObject("Generator");
        }
        BallGenerator bg = genObj.GetComponent<BallGenerator>();
        if (bg == null)
        {
            bg = genObj.AddComponent<BallGenerator>();
        }
        bg.ballPrefab = prefab;

        // 6. 今開いているシーンを保存
        EditorSceneManager.MarkSceneDirty(activeScene);
        EditorSceneManager.SaveScene(activeScene);

        Debug.Log($"【自動修正完了】 現在開いているシーン ({activeScene.name}) のカメラと設定を修復して保存しました！");
    }
}
