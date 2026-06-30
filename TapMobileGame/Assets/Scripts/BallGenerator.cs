using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    // 配列にすることで、複数のプレハブをInspectorから登録できるようになります
    public GameObject[] ballPrefabs;
    
    // 生成の間隔（秒）
    public float span = 1.0f;
    
    // 時間を測るための変数
    private float delta = 0;

    void Update()
    {
        this.delta += Time.deltaTime;

        // 🌟 焦らせるための追加コード：タイマーを取得する
        GameTimer timer = FindObjectOfType<GameTimer>();
        
        // 基本の出現ペース
        float currentSpan = this.span;

        if (timer != null)
        {
            // 残り時間に応じて出現ペースをどんどん速くする！
            if (timer.timeLimit <= 10.0f)
            {
                currentSpan = 0.4f; // 残り10秒以下：0.2秒に1個（猛吹雪モード！）
            }
            else if (timer.timeLimit <= 20.0f)
            {
                currentSpan = 0.7f; // 残り20秒以下：0.5秒に1個（少し焦る）
            }
        }

        // 基本のspanではなく、計算したcurrentSpanを使う
        if (this.delta > currentSpan)
        {
            this.delta = 0; // タイマーをリセットット

            // X座標を -2.0 ～ 2.0 の間でランダムに決める
            float randomX = Random.Range(-2.0f, 2.0f);
            float randomY = Random.Range(-2.0f, 2.0f);
            Vector3 spawnPos = new Vector3(randomX, randomY, 0);

            // プレハブが1つ以上登録されている場合のみ生成する
            if (ballPrefabs != null && ballPrefabs.Length > 0)
            {
                // 登録されたプレハブの中からランダムに1つ選ぶ
                int randomIndex = Random.Range(0, ballPrefabs.Length);
                GameObject selectedPrefab = ballPrefabs[randomIndex];

                // 選ばれたプレハブを生成
                Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}