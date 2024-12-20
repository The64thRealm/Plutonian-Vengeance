using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using USCG.Core.Telemetry;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Pluto pluto;
    public GameObject projectiles;
    public List<Asteroid> capturableAsteroids;

    private int finalLevelBuildID = 7;
    public int currentLevelBuildID;
    private float time;
    private int numAsteroidsCreated;
    private int numAsteroidsCaptured;
    private int numAsteroidsDidDamage;
    private int numTimesDamaged;
    private float totalDamageTaken;

    private static MetricId capturablesCreatedSources { get; set; }
    private static MetricId capturablesCapturedSources { get; set; }
    private static MetricId playerAsteroidDamages { get; set; }
    private static MetricId damagedReasons { get; set; }
    private static MetricId deathReasons { get; set; }
    private static MetricId levelTimes { get; set; }

    private void Awake()
    {
        if (instance != null)
        {
            // pass the projectile and pluto objects in
            instance.projectiles = projectiles;
            instance.pluto = pluto;

            Destroy(gameObject);
            return;
        }

        instance = this;
        resetPerRespawnStats();

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        capturableAsteroids = new();
        
        SceneManager.sceneLoaded += HandleSceneChange;
    }

    private void Start()
    {
        InitializeTelemetryMetrics();
        CheckForCurrentLevel();
    }

    private void CheckForCurrentLevel()
    {
        bool success = false;
        if (SceneManager.GetActiveScene().buildIndex <= finalLevelBuildID)
        {
            currentLevelBuildID = SceneManager.GetActiveScene().buildIndex;
            success = true;
        }
        LogLevelStats(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(currentLevelBuildID)), time, numAsteroidsCreated, numAsteroidsCaptured, numAsteroidsDidDamage, numTimesDamaged, totalDamageTaken, success);
        resetPerRespawnStats();
    }

    private void resetPerRespawnStats()
    {
        time = 0;
        numAsteroidsCreated = 0;
        numAsteroidsCaptured = 0;
        numAsteroidsDidDamage = 0;
        numTimesDamaged = 0;
        totalDamageTaken = 0;
    }

    private void InitializeTelemetryMetrics()
    {
        if (TelemetryManager.instance == null)
        {
            throw new System.Exception("Please add a telemetry manager into the scene!");
        }

        if (capturablesCreatedSources == null)
        {
            capturablesCreatedSources = TelemetryManager.instance.CreateSampledMetric<string>("Capturables created source");
        }
        if (capturablesCapturedSources == null)
        {
            capturablesCapturedSources = TelemetryManager.instance.CreateSampledMetric<string>("Capturables captured source");
        }
        if (playerAsteroidDamages == null)
        {
            playerAsteroidDamages = TelemetryManager.instance.CreateSampledMetric<string>("Player Asteroid Damage");
        }
        if (damagedReasons == null)
        {
            damagedReasons = TelemetryManager.instance.CreateSampledMetric<string>("Damaged Reasons");
        }
        if (deathReasons == null)
        {
            deathReasons = TelemetryManager.instance.CreateSampledMetric<string>("Death Reasons");
        }
        if (levelTimes == null)
        {
            levelTimes = TelemetryManager.instance.CreateSampledMetric<string>("Level Time");
        }
    }

    public void AsteroidCreated(Asteroid asteroid, string source)
    {
        TelemetryManager.instance.AddMetricSample<string>(capturablesCreatedSources, source);
        capturableAsteroids.Add(asteroid);
        ++numAsteroidsCreated;
    }

    public void AsteroidCaptured(Asteroid asteroid, string source)
    {
        TelemetryManager.instance.AddMetricSample<string>(capturablesCapturedSources, source);
        ++numAsteroidsCaptured;
    }

    public void AsteroidCollidedWithPlanet(string source, float damage)
    {
        TelemetryManager.instance.AddMetricSample<string>(playerAsteroidDamages, $"{source},{damage}");
        ++numAsteroidsDidDamage;
    }

    public void PlayerDamaged(string source, float damage)
    {
        TelemetryManager.instance.AddMetricSample<string>(damagedReasons, $"{source},{damage}");
        ++numTimesDamaged;
        totalDamageTaken += damage;
    }

    public void PlayerDied(string source, float damage)
    {
        TelemetryManager.instance.AddMetricSample<string>(deathReasons, $"{source},{damage}");
    }
    public void LogLevelStats(string level, float time, int numAsteroidsCreated, int numAsteroidsCaptured, int numAsteroidsDidDamage, int damageTaken, float instancesOfDamageTaken, bool success)
    {
        TelemetryManager.instance.AddMetricSample<string>(levelTimes, $"{level},{time},{numAsteroidsCreated},{numAsteroidsCaptured},{numAsteroidsDidDamage},{damageTaken},{instancesOfDamageTaken},{success}");
    }

    public void HandleSceneChange(Scene sceneLoaded, LoadSceneMode mode)
    {
        CheckForCurrentLevel();
    }

    private void Update()
    {
        time += Time.deltaTime;
    }
}
