using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    [SerializeField, Tooltip("揺れるスピード")]
    private float waveSpeed = 10.0f;
    [SerializeField, Tooltip("揺れる幅（ピクセル）")]
    private float waveAmplitude = 10.0f;
    private float startLocalY;
    private void Start()
    {
        //transform.localPosition.y; (UIの座標はLocalを使う)
        startLocalY = transform.localPosition.y;
    }
    private void Update()
    {
        // サイン波を使って滑らかに上下させる
        float y = Mathf.Sin(Time.time * waveSpeed) * waveAmplitude;
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            startLocalY + y,
            transform.localPosition.z // Z軸も元々の値を維持するのが安全です
        );
    }
}
