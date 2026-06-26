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
        if (this.delta > this.span)
        {
            this.delta = 0; // タイマーをリセット

            // X座標を -2.0 ～ 2.0 の間でランダムに決める
            float randomX = Random.Range(-2.0f, 2.0f);
            Vector3 spawnPos = new Vector3(randomX, 4.0f, 0);

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