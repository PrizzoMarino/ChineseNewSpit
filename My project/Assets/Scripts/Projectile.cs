using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public AudioClip WrongAnimal;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.up * speed;
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            if (enemy.zodiacType == GameManager.Instance.currentYear)
            {
                GameManager.Instance.score -= 10; // shot the one allowed in
                AudioSource.PlayClipAtPoint(WrongAnimal, transform.position);
            }
            else
                GameManager.Instance.score += 5;  // shot the one not allowed in

            GameManager.Instance.UpdateScoreUI();

            Destroy(enemy.gameObject);
            Destroy(gameObject);

        }
    }

}
