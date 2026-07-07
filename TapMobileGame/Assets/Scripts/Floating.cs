using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    private float moveSpeed = 1.0f;//上昇スピード

    private float lifeTime = 1.0f;//消えるまでの時間

    private TextMeshProUGUI textComponent;

    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
        Destroy(gameObject, lifeTime);//自動で削除

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (textComponent != null)
        {
            Color c = textComponent.color;
            c.a -= (1.0f / lifeTime) * Time.deltaTime;
            textComponent.color = c;
        }
        else
        {
            // もしここが表示されたら、スクリプトがテキストを見つけられていません！
            Debug.LogWarning("テキストコンポーネントが見つかりません！");
        }
    }
}
