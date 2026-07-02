using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public class RefactorAutomator
{
    [MenuItem("Tools/✨プロ仕様に修復（Missing対応）✨")]
    public static void RunRefactorCleanup()
    {
        // 1. Gameシーンを開いてジェネレーターのスクリプトを差し替え
        var gameScene = EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        GameObject generatorObj = GameObject.Find("Generator"); // 元の名前は Generator でした！
        if (generatorObj != null)
        {
            generatorObj.name = "TargetSpawner"; // オブジェクト名も規約に沿って変更
            
            // 古いコンポーネント（Missing）を削除
            var components = generatorObj.GetComponents<Component>();
            foreach (var c in components)
            {
                if (c == null) GameObject.DestroyImmediate(generatorObj.GetComponent(c.GetType()));
                // Unity APIではmissingはGetComponentで取れないため、SerializedObjectで消すのが安全ですが、
                // 簡単のために GameObjectUtility.RemoveMonoBehavioursWithMissingScript を使います。
            }
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(generatorObj);
            
            TargetSpawner spawner = generatorObj.GetComponent<TargetSpawner>();
            if (spawner == null)
            {
                spawner = generatorObj.AddComponent<TargetSpawner>();
            }
            
            // プレハブを割り当て
            GameObject ballPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Object/BallPrefab.prefab");
            if (ballPrefab != null)
            {
                SerializedObject so = new SerializedObject(spawner);
                so.Update();
                var arrayProp = so.FindProperty("targetPrefabs");
                arrayProp.arraySize = 1;
                arrayProp.GetArrayElementAtIndex(0).objectReferenceValue = ballPrefab;
                so.ApplyModifiedProperties();
            }
        }
        EditorSceneManager.MarkSceneDirty(gameScene);
        EditorSceneManager.SaveScene(gameScene);

        // 2. プレハブのスクリプト差し替え
        string prefabPath = "Assets/Object/BallPrefab.prefab";
        GameObject prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefabObj != null)
        {
            GameObject contentsRoot = PrefabUtility.LoadPrefabContents(prefabPath);

            // 古いMissingScriptを削除
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(contentsRoot);

            TargetController controller = contentsRoot.GetComponent<TargetController>();
            if (controller == null)
            {
                controller = contentsRoot.AddComponent<TargetController>();
            }

            // エフェクトだけ再設定（音はユーザーに任せるか、自動設定するか）
            GameObject effect = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Object/PopEffect.prefab");

            SerializedObject so = new SerializedObject(controller);
            so.Update();
            so.FindProperty("pointValue").intValue = 10;
            so.FindProperty("lifetime").floatValue = 1.0f;
            if (effect != null) so.FindProperty("effectPrefab").objectReferenceValue = effect;
            so.ApplyModifiedProperties();

            PrefabUtility.SaveAsPrefabAsset(contentsRoot, prefabPath);
            PrefabUtility.UnloadPrefabContents(contentsRoot);
        }

        // Titleに戻る
        EditorSceneManager.OpenScene("Assets/Scenes/Title.unity");

        Debug.Log("【修復完了】すべてのクラスと参照がプロ仕様（TargetController / TargetSpawner）にアップデートされました！");
    }
}
