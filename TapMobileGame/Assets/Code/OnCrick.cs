using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    // オブジェクトがクリックされた時に呼ばれる
    void OnMouseDown()
    {
        // ScoreManagerを探して10点追加する
        ScoreManager sm = FindObjectOfType<ScoreManager>();
        if (sm != null)
        {
            sm.AddScore(10);
        }

        // 自分自身(gameObject)を削除(Destroy)する
        Destroy(gameObject);
    }
}