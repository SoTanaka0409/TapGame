using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    public int pointValue = 10;
    public float lifetime = 1.0f;
    
    // エフェクト用のプレハブを入れる変数
    public GameObject effectPrefab; 

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnMouseDown()
    {
        ScoreManager sm = FindObjectOfType<ScoreManager>();
        if (sm != null)
        {
            sm.AddScore(pointValue);
        }

        // ★もしエフェクトが設定されていれば、自分がいる場所にエフェクトを出現させる
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
