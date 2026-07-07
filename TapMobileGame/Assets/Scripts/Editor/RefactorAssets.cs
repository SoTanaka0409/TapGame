using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class RefactorAssets
{
    [MenuItem("Tools/1. Refactor Assets and Scenes")]
    public static void RunRefactor()
    {
        // 1. フォルダ作成とファイル移動・リネーム
        if (!AssetDatabase.IsValidFolder("Assets/Sounds"))
        {
            AssetDatabase.CreateFolder("Assets", "Sounds");
        }

        RenameAndMoveAsset("Assets/BGM/gameBGM.mp3", "Assets/Sounds/bgm_game.mp3");
        RenameAndMoveAsset("Assets/SE/TapSe_Logo.mp3", "Assets/Sounds/se_logo.mp3");
        RenameAndMoveAsset("Assets/SE/TapSe_Object.mp3", "Assets/Sounds/se_tap.mp3");

        // プレハブのリネーム
        RenameAsset("Assets/Object/Ball/BallPrefab.prefab", "prefab_ball_normal");
        RenameAsset("Assets/Object/Ball/BallPrefab 1.prefab", "prefab_ball_green");
        RenameAsset("Assets/Object/Ball/BallPrefab 2.prefab", "prefab_ball_purple");
        RenameAsset("Assets/Object/Effect/ButtonEffect.prefab", "prefab_effect_button");
        RenameAsset("Assets/Object/Effect/PopEffect.prefab", "prefab_effect_pop");
        RenameAsset("Assets/Resources/FloatingTextPrefab.prefab", "prefab_floating_text");

        // 画像のリネーム
        RenameAsset("Assets/CircleTex.png", "img_circle_tex");
        RenameAsset("Assets/Object/Ball/Circle.png", "img_circle");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 2. シーン内のオブジェクト名修正
        string[] scenes = { "Assets/Scenes/Title.unity", "Assets/Scenes/Game.unity", "Assets/Scenes/Result.unity" };
        foreach (var scenePath in scenes)
        {
            if (File.Exists(scenePath))
            {
                var scene = EditorSceneManager.OpenScene(scenePath);
                GameObject[] roots = scene.GetRootGameObjects();
                foreach (var root in roots)
                {
                    RenameGameObjectRecursive(root);
                }
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }
        }

        Debug.Log("アセットとシーンのリファクタリングが完了しました！");
    }

    private static void RenameAndMoveAsset(string oldPath, string newPath)
    {
        if (AssetDatabase.LoadMainAssetAtPath(oldPath) != null)
        {
            string err = AssetDatabase.MoveAsset(oldPath, newPath);
            if (!string.IsNullOrEmpty(err)) Debug.LogError(err);
        }
    }

    private static void RenameAsset(string path, string newName)
    {
        if (AssetDatabase.LoadMainAssetAtPath(path) != null)
        {
            string err = AssetDatabase.RenameAsset(path, newName);
            if (!string.IsNullOrEmpty(err)) Debug.LogError(err);
        }
    }

    private static void RenameGameObjectRecursive(GameObject obj)
    {
        // 名前から全角、空白、括弧を排除し、PascalCaseっぽくする
        string newName = obj.name;
        newName = newName.Replace(" ", "").Replace("(", "").Replace(")", "");
        newName = newName.Replace("テキスト", "Text").Replace("スコア", "Score").Replace("ボタン", "Button").Replace("背景", "Background");
        
        // 最初の文字を大文字にする (PascalCase)
        if (newName.Length > 0)
        {
            newName = char.ToUpper(newName[0]) + newName.Substring(1);
        }
        
        // "Text1" のような場合、用途に合わせて変えたいが自動では難しいので最低限綺麗にする
        obj.name = newName;

        foreach (Transform child in obj.transform)
        {
            RenameGameObjectRecursive(child.gameObject);
        }
    }
}
