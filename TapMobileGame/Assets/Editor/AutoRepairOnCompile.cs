using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoRepairOnCompile
{
    static AutoRepairOnCompile()
    {
        // コンパイル直後に実行されるように予約
        EditorApplication.delayCall += RunRepair;
    }

    private static void RunRepair()
    {
        if (SessionState.GetBool("AutoRepairDone2", false)) return;
        SessionState.SetBool("AutoRepairDone2", true);

        Debug.Log("AutoRepairOnCompile 実行中...");
        
        // 1. Gameシーンを開いてジェネレーターの修復
        var gameScene = EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        
        // Generator または TargetSpawner を探す
        GameObject generatorObj = GameObject.Find("Generator");
        if (generatorObj == null) generatorObj = GameObject.Find("TargetSpawner");

        if (generatorObj != null)
        {
            generatorObj.name = "TargetSpawner";
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(generatorObj);
            
            TargetSpawner spawner = generatorObj.GetComponent<TargetSpawner>();
            if (spawner == null) spawner = generatorObj.AddComponent<TargetSpawner>();
            
            // プレハブ配列の再設定
            GameObject ballPrefab1 = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Object/Ball/BallPrefab.prefab");
            GameObject ballPrefab2 = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Object/Ball/BallPrefab 1.prefab");
            GameObject ballPrefab3 = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Object/Ball/BallPrefab 2.prefab");

            SerializedObject so = new SerializedObject(spawner);
            so.Update();
            var arrayProp = so.FindProperty("targetPrefabs");
            
            int count = 0;
            if (ballPrefab1 != null) count++;
            if (ballPrefab2 != null) count++;
            if (ballPrefab3 != null) count++;
            
            arrayProp.arraySize = count;
            
            int idx = 0;
            if (ballPrefab1 != null) arrayProp.GetArrayElementAtIndex(idx++).objectReferenceValue = ballPrefab1;
            if (ballPrefab2 != null) arrayProp.GetArrayElementAtIndex(idx++).objectReferenceValue = ballPrefab2;
            if (ballPrefab3 != null) arrayProp.GetArrayElementAtIndex(idx++).objectReferenceValue = ballPrefab3;
            
            so.ApplyModifiedProperties();
        }
        else
        {
            Debug.LogError("Generator オブジェクトが見つかりません！");
        }
        
        EditorSceneManager.MarkSceneDirty(gameScene);
        EditorSceneManager.SaveScene(gameScene);

        // 2. プレハブ自体のスクリプト修復
        string[] prefabPaths = {
            "Assets/Object/Ball/BallPrefab.prefab",
            "Assets/Object/Ball/BallPrefab 1.prefab",
            "Assets/Object/Ball/BallPrefab 2.prefab"
        };

        foreach (var path in prefabPaths)
        {
            GameObject pObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (pObj != null)
            {
                GameObject root = PrefabUtility.LoadPrefabContents(path);
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(root);
                
                TargetController ctrl = root.GetComponent<TargetController>();
                if (ctrl == null) ctrl = root.AddComponent<TargetController>();
                
                GameObject effect = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Object/PopEffect.prefab");
                
                SerializedObject so = new SerializedObject(ctrl);
                so.Update();
                so.FindProperty("pointValue").intValue = 10;
                so.FindProperty("lifetime").floatValue = 1.0f;
                if (effect != null) so.FindProperty("effectPrefab").objectReferenceValue = effect;
                so.ApplyModifiedProperties();
                
                PrefabUtility.SaveAsPrefabAsset(root, path);
                PrefabUtility.UnloadPrefabContents(root);
            }
        }

        // Titleシーンに戻す
        EditorSceneManager.OpenScene("Assets/Scenes/Title.unity");

        Debug.Log("【緊急修復完了】TargetSpawnerとTargetControllerのリンクを強制復旧しました！");
    }
}
