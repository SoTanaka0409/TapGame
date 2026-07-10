using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SetupSpaceDefender
{
    [MenuItem("Tools/2. Apply Space Defender Theme")]
    public static void ApplyTheme()
    {
        // 1. テクスチャの設定（Spriteに変更）
        SetTextureImporterSettings("Assets/Images/bg_space.png", TextureImporterType.Sprite);
        SetTextureImporterSettings("Assets/Images/spr_meteor.png", TextureImporterType.Sprite);
        AssetDatabase.Refresh();

        Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/bg_space.png");
        Sprite meteorSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/spr_meteor.png");

        if (bgSprite == null || meteorSprite == null)
        {
            Debug.LogError("背景または隕石の画像が見つかりません。");
            return;
        }

        // 2. マテリアルの作成（加算合成で黒背景を透過）
        string matPath = "Assets/Images/AdditiveSprite.mat";
        Material additiveMat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        if (additiveMat == null)
        {
            additiveMat = new Material(Shader.Find("Mobile/Particles/Additive"));
            AssetDatabase.CreateAsset(additiveMat, matPath);
        }

        // 3. 各シーンに背景を追加
        string[] scenes = { "Assets/Scenes/Title.unity", "Assets/Scenes/Game.unity", "Assets/Scenes/Result.unity" };
        foreach (var scenePath in scenes)
        {
            if (System.IO.File.Exists(scenePath))
            {
                var scene = EditorSceneManager.OpenScene(scenePath);
                
                // 既存のBackgroundがあれば削除
                GameObject oldBg = GameObject.Find("Background");
                if (oldBg != null) GameObject.DestroyImmediate(oldBg);

                // 新しいBackgroundを作成
                GameObject bgObj = new GameObject("Background");
                SpriteRenderer sr = bgObj.AddComponent<SpriteRenderer>();
                sr.sprite = bgSprite;
                sr.sortingOrder = -100;

                // 画面を覆うようにスケール調整（Orthographic Size 5 = 縦10）
                // 画像サイズに合わせて適当に拡大
                float cameraHeight = Camera.main != null ? Camera.main.orthographicSize * 2 : 10f;
                float spriteHeight = bgSprite.bounds.size.y;
                float scale = (cameraHeight / spriteHeight) * 1.2f; // 少し余裕を持たせる
                bgObj.transform.localScale = new Vector3(scale, scale, 1);
                
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }
        }

        // 4. ボールのプレハブを隕石に変更し、色を調整
        UpdateBallPrefab("Assets/Objects/Ball/prefab_ball_normal.prefab", meteorSprite, additiveMat, Color.white);
        UpdateBallPrefab("Assets/Objects/Ball/prefab_ball_green.prefab", meteorSprite, additiveMat, new Color(0.5f, 1f, 0.5f));
        UpdateBallPrefab("Assets/Objects/Ball/prefab_ball_purple.prefab", meteorSprite, additiveMat, new Color(1f, 0.5f, 1f));

        // 5. エフェクト（PopEffect）の色を宇宙っぽくする
        UpdateEffectPrefab("Assets/Objects/Effect/prefab_effect_pop.prefab");

        Debug.Log("宇宙テーマ（Space Defender）の適用が完了しました！");
    }

    private static void SetTextureImporterSettings(string path, TextureImporterType type)
    {
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = type;
            importer.SaveAndReimport();
        }
    }

    private static void UpdateBallPrefab(string path, Sprite sprite, Material mat, Color color)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab != null)
        {
            SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = sprite;
                sr.material = mat;
                sr.color = color;
                
                // 隕石画像は少し大きめかもしれないのでスケール調整
                prefab.transform.localScale = new Vector3(0.15f, 0.15f, 1f); 
            }
            PrefabUtility.SavePrefabAsset(prefab);
        }
    }

    private static void UpdateEffectPrefab(string path)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab != null)
        {
            ParticleSystem ps = prefab.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.startColor = new ParticleSystem.MinMaxGradient(Color.cyan, Color.magenta);
                var shape = ps.shape;
                shape.radius = 0.5f;
            }
            PrefabUtility.SavePrefabAsset(prefab);
        }
    }
}
