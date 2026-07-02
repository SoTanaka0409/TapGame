using UnityEngine;

/// <summary>
/// 定期的にターゲット（的）を画面内にランダムな位置で生成するクラス
/// </summary>
public class TargetSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("生成するターゲットのプレハブ配列")]
    private GameObject[] targetPrefabs;
    
    [SerializeField, Tooltip("基本となる生成間隔（秒）")]
    private float baseSpawnInterval = 1.0f;

    [SerializeField, Tooltip("残り10秒以下になった際の生成間隔（秒）")]
    private float frenzySpawnInterval = 0.5f;

    [SerializeField, Tooltip("残り20秒以下になった際の生成間隔（秒）")]
    private float cautionSpawnInterval = 0.8f;

    private const float WarningTimeThreshold = 10.0f;
    private const float CautionTimeThreshold = 20.0f;
    private const float SpawnAreaMin = -2.0f;
    private const float SpawnAreaMax = 2.0f;

    private float spawnTimer = 0f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        float currentInterval = CalculateCurrentSpawnInterval();

        if (spawnTimer > currentInterval)
        {
            spawnTimer = 0f;
            SpawnRandomTarget();
        }
    }

    /// <summary>
    /// 現在の残り時間に基づいて、ターゲットの生成間隔を計算する
    /// </summary>
    /// <returns>計算された生成間隔（秒）</returns>
    private float CalculateCurrentSpawnInterval()
    {
        float interval = baseSpawnInterval;
        GameTimer timer = FindObjectOfType<GameTimer>();

        if (timer != null)
        {
            if (timer.CurrentTimeLimit <= WarningTimeThreshold)
            {
                interval = frenzySpawnInterval;
            }
            else if (timer.CurrentTimeLimit <= CautionTimeThreshold)
            {
                interval = cautionSpawnInterval;
            }
        }

        return interval;
    }

    /// <summary>
    /// 設定されたプレハブの中からランダムに1つ選び、ランダムな位置に生成する
    /// </summary>
    private void SpawnRandomTarget()
    {
        if (targetPrefabs == null || targetPrefabs.Length == 0) return;

        float randomX = Random.Range(SpawnAreaMin, SpawnAreaMax);
        float randomY = Random.Range(SpawnAreaMin, SpawnAreaMax);
        Vector3 spawnPos = new Vector3(randomX, randomY, 0);

        int randomIndex = Random.Range(0, targetPrefabs.Length);
        GameObject selectedPrefab = targetPrefabs[randomIndex];

        if (selectedPrefab != null)
        {
            Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        }
    }
}
