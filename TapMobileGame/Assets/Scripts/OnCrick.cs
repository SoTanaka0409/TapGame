using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    public int pointValue = 10;
    public float lifetime = 1.0f;
    
    // ★エフェクト用のプレハブを入れる変数
    public GameObject effectPrefab; 

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 1. スマホのマルチタッチ対応
        // 画面に触れているすべての指（タッチ）を順番に調べる
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            
            // もし指が「画面に触れた瞬間」なら
            if (touch.phase == TouchPhase.Began)
            {
                // タッチした画面の座標を、ゲーム内の空間の座標に変換する
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                
                // その座標に自分自身（の当たり判定）があるか調べる
                Collider2D hitCollider = Physics2D.OverlapPoint(touchPos);
                if (hitCollider != null && hitCollider.gameObject == this.gameObject)
                {
                    BreakBall();
                }
            }
        }

        // 2. パソコン（Unityエディタ）でのテスト用（マウスの左クリック）
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);
            if (hitCollider != null && hitCollider.gameObject == this.gameObject)
            {
                BreakBall();
            }
        }
    }

    // ボールを消すときの処理（スコア追加＋エフェクト＋消去）
    void BreakBall()
    {
        ScoreManager sm = FindObjectOfType<ScoreManager>();
        if (sm != null)
        {
            sm.AddScore(pointValue);
        }

        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}