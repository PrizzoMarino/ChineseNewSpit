using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Year Settings")]
    public ZodiacType currentYear;
    public float yearDuration = 10f;
    private float yearTimer;

    public TextMeshProUGUI YearTimerText;
    public TextMeshProUGUI OverallTimeText;


    [Header("Spawning")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnRate = 1.5f;
    private float spawnTimer;

    [Header("Score")]
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI yearText;
    public GameObject gameOverText;

    private bool gameOver = false;

    void Awake()
    {
        Instance = this;
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
        YearTimerText.text = "Year ends in: " + Mathf.CeilToInt(timeLeft) + "s";

        // Overall time passed
        int totalSeconds = Mathf.FloorToInt(Time.timeSinceLevelLoad);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        OverallTimeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
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

        if (spawnTimer >= spawnRate)
        {
            spawnTimer = 0;
            SpawnEnemy();
        }
    }

    void SwapYear()
    {
        currentYear = (ZodiacType)Random.Range(0, 4);
        yearText.text = "YEAR OF THE: \n" + currentYear.ToString().ToUpper();
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
        if (enemy.zodiacType == currentYear)
        {
            score -= 10; // Shot the one you should allow in
        }
        else
        {
            score -= 5; // Shot the right one
        }

        UpdateScoreUI();
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
        gameOverText.SetActive(true);
        Time.timeScale = 0f;
    }
}
