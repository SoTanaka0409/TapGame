using UnityEngine;
using UnityEngine.SceneManagement; // シーン移動に必須

public class SceneChanger : MonoBehaviour
{
    // ボタンが押されたときに呼ばれるメソッド
    public void GoToGameScene()
    {
        // "GameScene" という名前のシーンを読み込んで移動する
        SceneManager.LoadScene("GameScene");
    }
}
