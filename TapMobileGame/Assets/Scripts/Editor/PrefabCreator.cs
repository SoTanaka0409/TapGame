using UnityEngine;
using UnityEditor;
using TMPro;

public class PrefabCreator : EditorWindow
{
    [MenuItem("Tools/✨メリハリ演出用プレハブを生成✨")]
    public static void CreateFloatingTextPrefab()
    {
        string path = "Assets/Resources/prefab_floating_text.prefab";

        // 空のGameObjectを作成
        GameObject obj = new GameObject("prefab_floating_text");
        
        // Add components
        TextMeshPro tmpro = obj.AddComponent<TextMeshPro>();
        tmpro.text = "+10";
        tmpro.alignment = TextAlignmentOptions.Center;
        tmpro.fontSize = 5;
        tmpro.color = Color.white;
        
        // Disable wrapping and adjust RectTransform so it's visible in 3D
        tmpro.enableWordWrapping = false;
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(5, 2);
        
        // Add FloatingText script
        obj.AddComponent<FloatingText>();

        // Ensure directory exists
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        // Save as prefab
        PrefabUtility.SaveAsPrefabAsset(obj, path);
        DestroyImmediate(obj);

        Debug.Log("prefab_floating_text を生成しました！: " + path);
    }
}
