using UnityEngine;
using TMPro;

// 設計ルール：スコア獲得時の高揚感を演出しつつ、動的な文字生成によるオーバーヘッドを抑えるための3D浮遊テキスト制御クラス
public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1.5f;   // 画面外へのハミ出しを防ぎ、プレイヤーが視認しやすい等速上昇スピードの基準値
    public float lifetime = 1.0f;    // 画面上のテキスト過多による乱雑化（クランピング）を防ぐための生存限界時間（秒）
    private TextMeshPro textComponent; // 毎フレームのコンポーネント検索によるCPU負荷（スパイク）を防ぐためのキャッシュ変数

    void Start()
    {
        // バグ回避：親子関係やPrefabの構造変更によってテキストコンポーネントが最上位からズレた場合のエラー防止
        textComponent = GetComponent<TextMeshPro>();
        if (textComponent == null)
            textComponent = GetComponentInChildren<TextMeshPro>();

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (textComponent != null)
        {
            Color currentColor = textComponent.color;
            // 物理仕様：生存時間（lifetime）が経過した瞬間に完全に見えなくなるよう、経過時間の逆数比率で綺麗にフェードアウトさせる
            currentColor.a -= (1.0f / lifetime) * Time.deltaTime;
            textComponent.color = currentColor;
        }
    }

    // 入力：score=獲得したスコア値
    // 副作用：テキスト文字の書き換え、スコアランクに応じた文字色およびフォントサイズのリアルタイム変更
    public void Setup(int score)
    {
        // バグ回避：Startが実行される前（インスタンス化直後）に外部からSetupが呼ばれた際のNullReferenceException（ヌルポバグ）を防御する
        textComponent = GetComponent<TextMeshPro>();
        if (textComponent == null) return;

        textComponent.text = "+" + score;

        // 業務ルール：3桁以上の大物（牛）やコンボ継続時の報酬スコアに気づかせ、プレイヤーの達成感を刺激するための3段階フォント演出
        if (score >= 30)
        {
            textComponent.color = new Color(0.8f, 0.2f, 1f); // フィーバー中やハイスコア獲得を象徴する紫色
            textComponent.fontSize = 8;
        }
        else if (score >= 20)
        {
            textComponent.color = new Color(0.2f, 1f, 0.4f); // 連続捕獲（コンボ中）の成功を示す緑色
            textComponent.fontSize = 6;
        }
        else
        {
            textComponent.color = Color.white;
            textComponent.fontSize = 5;
        }
    }
}