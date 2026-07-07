using System.Collections;
using UnityEngine;

/// <summary>
/// タップされる的（ターゲット）の挙動、タップ時のスコア加算、エフェクト生成、消滅処理を管理するクラス
/// </summary>
public class TargetController : MonoBehaviour
{
    [SerializeField, Tooltip("このターゲットをタップした際に獲得できるスコア")]
    private int pointValue = 10;

    [SerializeField, Tooltip("ターゲットが自然消滅するまでの寿命（秒）")]
    private float lifetime = 1.0f;

    [SerializeField, Tooltip("タップ時に再生される効果音")]
    private AudioClip popSound;

    [SerializeField, Tooltip("タップ時に生成されるパーティクルエフェクトのプレハブ")]
    private GameObject effectPrefab; 

    private const float SpawnAnimDuration = 0.2f;
    private const float TapDeathAnimDuration = 0.1f;
    private const float AutoDeathAnimDuration = 0.2f;
    private const float EffectLifetime = 1.0f;
    private const float WarningTimeThreshold = 10.0f;
    private const float CautionTimeThreshold = 20.0f;

    private bool isDying = false;

    // Pitch modification
    private static float currentPitch = 1.0f;
    private static float lastTapTime = 0f;

    private void Start()
    {
        StartCoroutine(SpawnAnimationRoutine());
        StartCoroutine(AutoDeathAnimationRoutine(lifetime));
    }

    private void Update()
    {
        if (isDying) return;

        // マルチタッチ対応
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                CheckTouchPosition(touch.position);
            }
        }

        // PC（エディタ）でのテスト用マウスクリック対応
        if (Input.GetMouseButtonDown(0))
        {
            CheckTouchPosition(Input.mousePosition);
        }
    }

    /// <summary>
    /// 画面上のタップ/クリック座標がこのターゲットと交差しているか判定する
    /// </summary>
    /// <param name="screenPos">画面座標</param>
    private void CheckTouchPosition(Vector2 screenPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPos);
        GameTimer timer = FindObjectOfType<GameTimer>();

        if (hitCollider != null && hitCollider.gameObject == this.gameObject&&timer.gameFinishFlag==false)
        {
            BreakTarget();
        }
    }

    /// <summary>
    /// ターゲットが破壊された際のスコア加算、エフェクト生成、SE再生、消滅アニメーションを開始する
    /// </summary>
    private void BreakTarget()
    {
        isDying = true;

        ScoreManager sm = FindObjectOfType<ScoreManager>();
        if (sm != null)
        {
            sm.AddScore(pointValue);
        }

        SpawnEffect();
        PlaySound();
        SpawnFloatingText();

        if (pointValue >= 30)
        {
            StartCoroutine(ShakeCamera(0.2f, 0.1f));
        }

        StartCoroutine(ShrinkAndDestroyRoutine(TapDeathAnimDuration));
    }

    private void SpawnFloatingText()
    {
        GameObject ftPrefab = Resources.Load<GameObject>("FloatingTextPrefab");
        if (ftPrefab != null)
        {
            GameObject ftObj = Instantiate(ftPrefab, transform.position, Quaternion.identity);
            FloatingText ft = ftObj.GetComponent<FloatingText>();
            if (ft != null)
            {
                ft.Setup(pointValue);
            }
        }
    }

    private System.Collections.IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }

    /// <summary>
    /// パーティクルエフェクトを生成し、残り時間に応じて色を変化させる
    /// </summary>
    private void SpawnEffect()
    {
        if (effectPrefab == null) return;

        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        GameTimer timer = FindObjectOfType<GameTimer>();

        if (ps != null && timer != null)
        {
            var main = ps.main;
            if (timer.CurrentTimeLimit <= WarningTimeThreshold)
            {
                main.startColor = Color.red;
            }
            else if (timer.CurrentTimeLimit <= CautionTimeThreshold)
            {
                main.startColor = new Color(1.0f, 0.5f, 0.0f); 
            }
            else
            {
                main.startColor = Color.white;
            }
        }

        Destroy(effect, EffectLifetime);
    }

    /// <summary>
    /// タップ時のSEを再生する
    /// </summary>
    private void PlaySound()
    {
        if (popSound != null)
        {
            if (Time.time - lastTapTime < 1.0f)
            {
                currentPitch = Mathf.Min(currentPitch + 0.1f, 2.0f);
            }
            else
            {
                currentPitch = 1.0f;
            }
            lastTapTime = Time.time;

            GameObject audioObj = new GameObject("TempAudio");
            audioObj.transform.position = Camera.main.transform.position;
            AudioSource source = audioObj.AddComponent<AudioSource>();
            
            source.clip = popSound;
            source.pitch = currentPitch;
            source.Play();
            
            Destroy(audioObj, popSound.length);
        }
    }

    /// <summary>
    /// 出現時のスケール拡大アニメーション
    /// </summary>
    private IEnumerator SpawnAnimationRoutine()
    {
        Vector3 finalScale = transform.localScale; 
        transform.localScale = Vector3.zero;

        float time = 0;
        while (time < SpawnAnimDuration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, finalScale, time / SpawnAnimDuration);
            yield return null;
        }
        transform.localScale = finalScale;
    }

    /// <summary>
    /// 寿命による自動消滅アニメーションの予約
    /// </summary>
    /// <param name="delay">寿命（秒）</param>
    private IEnumerator AutoDeathAnimationRoutine(float delay)
    {
        yield return new WaitForSeconds(delay - AutoDeathAnimDuration);
        
        if (!isDying) 
        {
            StartCoroutine(ShrinkAndDestroyRoutine(AutoDeathAnimDuration));
        }
    }

    /// <summary>
    /// 縮小しながら消滅する共通アニメーション
    /// </summary>
    /// <param name="duration">アニメーションにかける時間</param>
    private IEnumerator ShrinkAndDestroyRoutine(float duration)
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

        Destroy(gameObject);
    }
}
