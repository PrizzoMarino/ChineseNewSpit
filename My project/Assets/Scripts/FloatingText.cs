using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float lifetime = 1f;

    private TextMeshProUGUI text;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Setup(string message, Color color)
    {
        text.text = message;
        text.color = color;
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        lifetime -= Time.deltaTime;

        canvasGroup.alpha = lifetime;

        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
