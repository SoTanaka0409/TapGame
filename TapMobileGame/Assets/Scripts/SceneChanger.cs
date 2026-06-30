using UnityEngine;
using UnityEngine.SceneManagement; // シーン移動に必須

public class SceneChanger : MonoBehaviour
{
    // ボタンが押されたときに呼ばれるメソッド
    public void GoToGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}
