using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Spit System")]
    [Header("Spit Settings")]
    public int maxSpit = 5;
    public int currentSpit = 5;

    [Header("Water refill")]
    public int waterClicksNeeded = 5;
    private int waterClicks = 0;


    public int drinksNeeded = 5;
    private int currentDrinks = 0;
    private bool isRefilling = false;

    [Header("Spit UI")]
    public Image spitBarFill;

    [Header("Drinking")]
    public GameObject bottlePrefab;
    public Transform mouthPosition;
    public AudioClip drinkSound;
    public float drinkDuration = 0.5f;

    private bool isDrinking = false;


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

        if (Input.GetMouseButtonDown(1))
        {
            if (currentSpit == 0)
            {
                waterClicks++;
                StartCoroutine(DrinkRoutine());

                
                if (waterClicks >= waterClicksNeeded)
                {
                    currentSpit = maxSpit;
                    waterClicks = 0;
                    UpdateSpitUI();
                }
            }
            else
            {
                // If not at 0, allow normal refill or increment logic
                currentSpit = Mathf.Min(currentSpit + 1, maxSpit);
                UpdateSpitUI();
                StartCoroutine(DrinkRoutine());
            }
        }

        Rotate();
        Shoot();
    }
    public void UpdateSpitUI()
    {
        float fillAmount = (float)currentSpit / maxSpit;
        spitBarFill.fillAmount = fillAmount;
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

    IEnumerator DrinkRoutine()
    {
        if (isDrinking) yield break;
        isDrinking = true;

        GameObject bottle = Instantiate(bottlePrefab, mouthPosition.position, Quaternion.identity, transform);

        if (drinkSound != null)
            AudioSource.PlayClipAtPoint(drinkSound, transform.position);

        yield return new WaitForSeconds(drinkDuration);

        Destroy(bottle);
        isDrinking = false;
    }




}
