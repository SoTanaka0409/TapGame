using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SetBlackBackground
{
    [MenuItem("Tools/3. Set Black Background")]
    public static void ApplyBlackBackground()
    {
        string[] scenes = { "Assets/Scenes/Title.unity", "Assets/Scenes/Game.unity", "Assets/Scenes/Result.unity" };
        foreach (var scenePath in scenes)
        {
            if (System.IO.File.Exists(scenePath))
            {
                var scene = EditorSceneManager.OpenScene(scenePath);
                
                // 画像背景があれば削除
                GameObject oldBg = GameObject.Find("Background");
                if (oldBg != null)
                {
                    GameObject.DestroyImmediate(oldBg);
                }

                // カメラの背景色を真っ黒に変更
                if (Camera.main != null)
                {
                    Camera.main.clearFlags = CameraClearFlags.SolidColor;
                    Camera.main.backgroundColor = Color.black;
                }
                
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }
        }

        Debug.Log("すべてのシーンの背景を「真っ黒」に変更しました！");
    }
}
