using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    float startY;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Mathf.Sin Ç≈è„â∫Ç…Ç‰Ç¡Ç≠ÇËóhÇÍÇÈ
        float y = Mathf.Sin(Time.time * 2.0f) * 10f;

        transform.localPosition = new Vector3(
            transform.localPosition.x,
           startY + y,  // startYÇÕèâä˙Yç¿ïW
            0
        );
    }
}
