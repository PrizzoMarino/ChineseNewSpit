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

    public Sprite dragonSprite;
    public Sprite horseSprite;
    public Sprite roosterSprite;
    public Sprite pigSprite;

    public void SetSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        switch (zodiacType)
        {
            case ZodiacType.Dragon:
                sr.sprite = dragonSprite;
                break;

            case ZodiacType.Horse:
                sr.sprite = horseSprite;
                break;

            case ZodiacType.Rooster:
                sr.sprite = roosterSprite;
                break;

            case ZodiacType.Pig:
                sr.sprite = pigSprite;
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
