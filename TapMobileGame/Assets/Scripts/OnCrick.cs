using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    // インスペクターからこのオブジェクトの得点を自由に設定できるようにする
    public int pointValue = 10;

    // 何秒で自動的に消滅するかを設定できるようにする
    public float lifetime = 1.0f;

    void Start()
    {
        // 画面に出現した（Startが呼ばれた）と同時に、自動消滅のタイマーをセットする
        // 第二引数に「秒数」を入れるだけで、Unityが勝手に時間を測って消してくれます！
        Destroy(gameObject, lifetime);
    }

    // オブジェクトがクリックされた時に呼ばれる
    void OnMouseDown()
    {
        // ScoreManagerを探して、自身の持っている得点(pointValue)を追加する
        ScoreManager sm = FindObjectOfType<ScoreManager>();
        if (sm != null)
        {
            sm.AddScore(pointValue);
        }

        // クリックされたら即座に自分自身(gameObject)を削除(Destroy)する
        Destroy(gameObject);
    }
}