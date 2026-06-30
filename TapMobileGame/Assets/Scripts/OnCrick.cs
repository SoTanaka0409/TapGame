using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    public int pointValue = 10;
    public float lifetime = 1.0f;
    public AudioClip PopSound;
    public GameObject effectPrefab; 

    // アニメーション中（消えかけ）かどうかを判定するフラグ
    private bool isDying = false;

    void Start()
    {
        // 1. 出現アニメーションをスタート
        StartCoroutine(SpawnAnimation());
        
        // 2. 寿命（lifetime）が尽きる時の消滅アニメーションを予約
        StartCoroutine(AutoDeathAnimation(lifetime));
    }

    void Update()
    {
        // 既に消えかけている場合はタッチ判定をしない
        if (isDying) return;

        // スマホのマルチタッチ対応
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D hitCollider = Physics2D.OverlapPoint(touchPos);
                if (hitCollider != null && hitCollider.gameObject == this.gameObject)
                {
                    BreakBall();
                }
            }
        }

        // パソコンでのテスト用
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

    void BreakBall()
    {
        isDying = true; // タップされたので「消えかけ」状態にする

        ScoreManager sm = FindObjectOfType<ScoreManager>();
        if (sm != null)
        {
            sm.AddScore(pointValue);
        }

        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            GameTimer timer = FindObjectOfType<GameTimer>();

            if (ps != null && timer != null)
            {
                var main = ps.main;
                if (timer.timeLimit <= 10.0f) main.startColor = Color.red;
                else if (timer.timeLimit <= 20.0f) main.startColor = new Color(1.0f, 0.5f, 0.0f); 
                else main.startColor = Color.white;

               
            }
            Destroy(effect, 1.0f);
        }
        if (PopSound != null)
        {
            AudioSource.PlayClipAtPoint(PopSound, Camera.main.transform.position);
        }

        // 即座に消さずに、縮んで消えるアニメーションをスタート
        StartCoroutine(ShrinkAndDestroy(0.1f));
    }

    // ==========================================
    // アニメーション用の魔法（コルーチン）
    // ==========================================

    // 出現する時のアニメーション（0.2秒かけてポンッと大きくなる）
    IEnumerator SpawnAnimation()
    {
        Vector3 finalScale = transform.localScale; // 元の大きさを記憶
        transform.localScale = Vector3.zero;       // 最初は見えない大きさにする

        float time = 0;
        float duration = 0.2f;

        while (time < duration)
        {
            time += Time.deltaTime;
            // だんだん大きくする
            transform.localScale = Vector3.Lerp(Vector3.zero, finalScale, time / duration);
            yield return null; // 次のフレームまで待つ
        }
        transform.localScale = finalScale;
    }

    // 時間切れで消える時のアニメーション
    IEnumerator AutoDeathAnimation(float delay)
    {
        // 寿命の0.2秒前まで待機する
        yield return new WaitForSeconds(delay - 0.2f);
        
        // もしまだタップされていなければ、縮んで消える
        if (!isDying) 
        {
            StartCoroutine(ShrinkAndDestroy(0.2f));
        }
    }

    // シュッと縮んで消滅する共通アニメーション
    IEnumerator ShrinkAndDestroy(float duration)
    {
        isDying = true;
        Vector3 startScale = transform.localScale;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, time / duration);
            yield return null;
        }

        Destroy(gameObject); // 最後に完全に消去する
    }
}