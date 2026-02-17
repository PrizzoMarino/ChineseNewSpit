using UnityEngine;

public class Enemy : MonoBehaviour
{
    public ZodiacType zodiacType;
    public float speed = 2f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    public void SetColor()
    {
        // THIS IS ONLY FOR NOW
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        switch (zodiacType)
        {
            case ZodiacType.Dragon:
                sr.color = Color.red;
                break;
            case ZodiacType.Horse:
                sr.color = Color.blue;
                break;
            case ZodiacType.Rooster:
                sr.color = Color.yellow;
                break;
            case ZodiacType.Pig:
                sr.color = new Color(1f, 0.4f, 0.7f);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (zodiacType != GameManager.Instance.currentYear)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
