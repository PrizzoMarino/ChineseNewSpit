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
            GameManager.Instance.HandleEnemyShot(enemy);

            Destroy(enemy.gameObject);
            Destroy(gameObject);
        }

    }

}
