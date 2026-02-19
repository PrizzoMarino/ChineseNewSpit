using UnityEngine;

public class ScoreRadius : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null) return;

        GameManager.Instance.HandleEnemyAura(enemy);

        enemy.PlayDeathSound();
        Destroy(enemy.gameObject);
    }
}
