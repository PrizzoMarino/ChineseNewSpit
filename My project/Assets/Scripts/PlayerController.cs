using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 200f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    void Update()
    {
        Rotate();
        Shoot();
    }

    void Rotate()
    {
        float input = Input.GetAxisRaw("Horizontal"); 
        transform.Rotate(Vector3.forward * -input * rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }
}
