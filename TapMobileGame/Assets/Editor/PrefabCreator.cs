using UnityEngine;
using UnityEditor;
using TMPro;

public class PrefabCreator : EditorWindow
{
    [MenuItem("Tools/✨メリハリ演出用プレハブを生成✨")]
    public static void CreateFloatingTextPrefab()
    {
        string path = "Assets/Resources/FloatingTextPrefab.prefab";

        // Create GameObject
        GameObject obj = new GameObject("FloatingTextPrefab");
        
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

        Debug.Log("FloatingTextPrefab を生成しました！: " + path);
    }
}
