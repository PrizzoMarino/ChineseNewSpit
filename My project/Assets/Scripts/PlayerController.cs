using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Spit System")]
    public int maxSpit = 10;
    public int currentSpit;

    public int drinksNeeded = 5;
    private int currentDrinks = 0;
    private bool isRefilling = false;

    [Header("Spit UI")]
    public Image spitBarFill;


    [Header("Audio")]
    public AudioClip shootClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentSpit = maxSpit;
        UpdateSpitUI();
    }


    void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.IsGameOver()) return;
        Rotate();
        Shoot();
    }
    void UpdateSpitUI()
    {
        if (spitBarFill == null) return;

        float percent = (float)currentSpit / maxSpit;
        spitBarFill.fillAmount = percent;

        if (percent <= 0)
            spitBarFill.color = Color.red;
        else
            spitBarFill.color = Color.cyan;
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
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isRefilling) return;   // can't shoot while refilling

            if (currentSpit > 0)
            {
                Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

                currentSpit--;
                UpdateSpitUI();

                if (shootClip != null)
                    audioSource.PlayOneShot(shootClip);

                if (currentSpit <= 0)
                {
                    isRefilling = true;
                    Debug.Log("Out of spit! Drink water!");
                }
            }
            else
            {
                Debug.Log("No spit left!");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!isRefilling) return;

            currentDrinks++;
            Debug.Log("Drinking... " + currentDrinks + "/" + drinksNeeded);

            if (currentDrinks >= drinksNeeded)
            {
                currentSpit = maxSpit;
                UpdateSpitUI();

                currentDrinks = 0;
                isRefilling = false;

                Debug.Log("Spit refilled!");
            }
        }
    }



}
