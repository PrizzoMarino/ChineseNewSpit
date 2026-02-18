using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.IsGameOver()) return;
        Rotate();
        Shoot();
    }

    void Rotate()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (input == Vector2.zero) return;

        // Determine direction using directions
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // Horizontal rotation
            if (input.x > 0)
                transform.rotation = Quaternion.Euler(0, 0, -90); // Right
            else
                transform.rotation = Quaternion.Euler(0, 0, 90); // Left
        }
        else
        {
            // Vertical rotations
            if (input.y > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0); // Up
            else
                transform.rotation = Quaternion.Euler(0, 0, 180); // Down
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }
}
