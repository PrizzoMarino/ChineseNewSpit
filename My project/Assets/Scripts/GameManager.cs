using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Year Settings")]
    public ZodiacType currentYear;
    private ZodiacType previousYear;
    public float yearDuration = 10f;
    private float yearTimer;

    public TextMeshProUGUI YearTimerText;
    public TextMeshProUGUI OverallTimeText;


    [Header("Spawning")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public float spawnRate = 1.5f;
    public float minSpawnRate = 0.3f;

    private float spawnTimer;


    [Header("Score")]
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI yearText;
    public GameObject gameOverText;

    [Header("Floating Text")]
    public GameObject floatingTextPrefab;
    public Canvas gameCanvas;

    private bool gameOver = false;
    public SpriteRenderer scoreRadiusSprite;

    [Header("Game Over")]
    public AudioClip gameOverClip;
    private AudioSource audioSource;

    public GameObject gameOverPanel;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }


    void Start()
    {
        SwapYear();
        UpdateScoreUI();
        gameOverText.SetActive(false);
    }

    void Update()
    {
        if (gameOver) return;

        HandleYearTimer();
        HandleSpawning();
        UpdateUI();
    }

    void UpdateUI()
    {
        // Year countdown
        float timeLeft = yearDuration - yearTimer;
        YearTimerText.text = "Year ends in: \n" + Mathf.CeilToInt(timeLeft) + "s";

        // Overall time passed
        int totalSeconds = Mathf.FloorToInt(Time.timeSinceLevelLoad);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        OverallTimeText.text = string.Format("Time: \n{0:00}:{1:00}", minutes, seconds);
    }


    void HandleYearTimer()
    {
        yearTimer += Time.deltaTime;

        if (yearTimer >= yearDuration)
        {
            yearTimer = 0;
            SwapYear();
        }
    }

    void HandleSpawning()
    {
        spawnTimer += Time.deltaTime;

        float currentSpawnRate = GetCurrentSpawnRate();

        if (spawnTimer >= currentSpawnRate)
        {
            spawnTimer = 0;
            SpawnEnemy();
        }
    }

    float GetCurrentSpawnRate()
    {
        float difficultyMultiplier = 1f + Time.timeSinceLevelLoad / 60f;

        float scaledSpawnRate = spawnRate / difficultyMultiplier;

        return Mathf.Clamp(scaledSpawnRate, minSpawnRate, spawnRate);
    }

    void SwapYear()
    {
        ZodiacType newYear;

        // Keep picking a random year until it's different from previous
        do
        {
            newYear = (ZodiacType)Random.Range(0, 4);
        }
        while (newYear == previousYear);

        currentYear = newYear;
        previousYear = currentYear;

        Color animalColor = GetColorForZodiac(currentYear);

        // Converting the color to HEX string for TMP (i love unity)
        string hexColor = ColorUtility.ToHtmlStringRGB(animalColor);
        yearText.text = $"YEAR OF THE: \n<color=#{hexColor}>{currentYear.ToString().ToUpper()}</color>";

        yearText.transform.localScale = Vector3.one * 1.5f;
        LeanTween.scale(yearText.gameObject, Vector3.one, 0.3f).setEaseOutBack();

        scoreRadiusSprite.color = animalColor;

        // Destroy all enemies when year changes
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in allEnemies)
        {
            Destroy(enemy.gameObject);
        }
    }



    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        GameObject enemyObj = Instantiate(
            enemyPrefab,
            spawnPoints[spawnIndex].position,
            spawnPoints[spawnIndex].rotation
        );

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.zodiacType = (ZodiacType)Random.Range(0, 4);
        enemy.SetSprite();

        // Increase speed over time (2x every 1min)
        float speedMultiplier = 1f + Time.timeSinceLevelLoad / 60f;
        enemy.speed = enemy.speed * speedMultiplier;
    }


    public void HandleEnemyShot(Enemy enemy)
    {
        Vector3 worldPosition = enemy.transform.position;

        bool correct;
        int points;

        if (enemy.zodiacType == currentYear)
        {
            points = -10;
            correct = false;
        }
        else
        {
            points = 5;
            correct = true;
        }

        score += points;
        UpdateScoreUI();

        ShowFloatingText(points, worldPosition, correct);
    }

    public Color GetColorForZodiac(ZodiacType type)
    {
        switch (type)
        {
            case ZodiacType.Dragon:
                return Color.green;

            case ZodiacType.Horse:
                return new Color(0.6f, 0.3f, 0.1f);

            case ZodiacType.Rooster:
                return Color.yellow;

            case ZodiacType.Pig:
                return Color.magenta;

            default:
                return Color.white;
        }
    }


    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }
    public bool IsGameOver()
    {
        return gameOver;
    }

    public void GameOver()
    {
        gameOver = true;

        MusicManager music = FindObjectOfType<MusicManager>();
        if (music != null)
            music.StopMusic();

        if (gameOverClip != null)
            audioSource.PlayOneShot(gameOverClip);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.SetActive(true);


        Time.timeScale = 0f;
    }

    public void ShowFloatingText(int amount, Vector3 worldPosition, bool correct)
    {
        GameObject obj = Instantiate(floatingTextPrefab, gameCanvas.transform);

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        obj.transform.position = screenPosition;

        FloatingText ft = obj.GetComponent<FloatingText>();

        Color color = correct ? Color.green : Color.red;

        ft.Setup(amount.ToString(), color);

    }

    public void HandleEnemyAura(Enemy enemy)
    {
        Vector3 worldPosition = enemy.transform.position;

        int points = 10;

        score += points;
        UpdateScoreUI();
        ShowFloatingText(points, worldPosition, true);
    }



}
