using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float lifetime = 1.0f;
    private TextMeshPro textComponent;

    void Start()
    {
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
            currentColor.a -= (1.0f / lifetime) * Time.deltaTime;
            textComponent.color = currentColor;
        }
    }

    public void Setup(int score)
    {
        textComponent = GetComponent<TextMeshPro>();
        if (textComponent == null) return;

        textComponent.text = "+" + score;

        if (score >= 30)
        {
            textComponent.color = new Color(0.8f, 0.2f, 1f); // Purple-ish
            textComponent.fontSize = 8;
        }
        else if (score >= 20)
        {
            textComponent.color = new Color(0.2f, 1f, 0.4f); // Green-ish
            textComponent.fontSize = 6;
        }
        else
        {
            textComponent.color = Color.white;
            textComponent.fontSize = 5;
        }
    }
}
