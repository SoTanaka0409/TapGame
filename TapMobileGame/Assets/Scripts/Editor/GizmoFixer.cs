using UnityEditor;
using UnityEngine;

public class GizmoFixer
{
    [MenuItem("Tools/✨ 白い線を消す（強制オフ） ✨")]
    public static void DisableGizmos()
    {
        var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
        var window = EditorWindow.GetWindow(type);
        if (window != null)
        {
            var gizmosField = type.GetField("m_Gizmos", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (gizmosField != null)
            {
                gizmosField.SetValue(window, false);
                window.Repaint();
                Debug.Log("【成功】Gameビューの枠線（Gizmos）を強制的にオフにしました！これで動画に線は映りません。");
            }
        }
    }
}
