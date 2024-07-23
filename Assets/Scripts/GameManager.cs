using System.Collections;
using System.Collections.Generic;
using EditorUtilities.CustomAttributes;
using UnityEngine;
using Utility;
using Debug = System.Diagnostics.Debug;

public class GameManager : Singleton<GameManager>
{
    private const float DistanceToKeepFromBonusElements = 0.75f;

    [Header("External References")]
    [SerializeField] private UIScore uiScore;

    [Header("Game Configuration")] 
    [SerializeField] private int maxPlayerLives = 3;
    [SerializeField] private int scorePerSecond = 1000; //1 punto ogni millesimo di secondo = 1000 punti al secondo
    [SerializeField] private float maxGameDurationSeconds = 300;
    [SerializeField, Range(0, 100)] private float onScoreLossSlowdownPercentage = 20;
    [SerializeField] private List<ObstacleConfiguration> _allObstacles;
    [SerializeField] private List<ObstacleConfiguration> _allBonuses;
    
    [Header("Obstacle Spawn Intervals")]
    [SerializeField, Tooltip("In Seconds")] private float minObstacleSpawnInterval = 0.2f;
    [SerializeField, Tooltip("In Seconds")] private float maxObstacleSpawnInterval = 0.6f;
    [SerializeField] private float timeToFastestObstacleSpawnInterval = 120;

    [Header("Bonus Spawn Intervals")]
    [SerializeField] private int bonusSpawnSequenceSize = 4;
    [SerializeField, Tooltip("In Seconds")] private float minBonusSpawnInterval = 1;
    [SerializeField, Tooltip("In Seconds")] private float maxBonusSpawnInterval = 5;
    [SerializeField] private float timeToFastestBonusSpawnInterval = 300;
    
    
    
    [ReadOnly] private float _currentScore;
    [ReadOnly] private float _currentGameDuration;

    public float NormalizedMaxTime => Mathf.Clamp(_currentGameDuration / maxGameDurationSeconds, 0, 1);
    public float NormalizedObstacleSpawnTime => Mathf.Clamp(_currentGameDuration / timeToFastestObstacleSpawnInterval, 0, 1);
    public float NormalizedBonusSpawnTime => Mathf.Clamp(_currentGameDuration / timeToFastestBonusSpawnInterval, 0, 1);

    private Vector2 _minSpawnCoordinate;
    private Vector2 _maxSpawnCoordinate;
    private Camera _cam;

    private bool _bonusElementsSpawning;
    private float _bonusElementsXPosSpawn;

    private void Awake()
    {
        _cam = Camera.main;
        Debug.Assert(_cam != null, nameof(_cam) + " != null");
        _minSpawnCoordinate = _cam.ScreenToWorldPoint(new Vector3(0, Screen.height));
        _maxSpawnCoordinate = _cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        //Increase offset by 1 to ensure items spawn off-screen
        _minSpawnCoordinate.y += 1;
        _maxSpawnCoordinate.y += 1;
    }

    private void Start()
    {
        StartCoroutine(SpawnObstacles());
        StartCoroutine(SpawnBonusElements());
    }

    private IEnumerator SpawnBonusElements()
    {
        while (maxPlayerLives >= 0)
        {
            _bonusElementsSpawning = true;
            float rngXPos = Random.Range(_minSpawnCoordinate.x, _maxSpawnCoordinate.x);
            float yPos = _minSpawnCoordinate.y;
            
            _bonusElementsXPosSpawn = rngXPos;

            Vector2 spawnPoint = new Vector2(rngXPos, yPos);
            ObstacleConfiguration obstacleConfig = _allBonuses.GetRandom();
            GameObject obstaclePrefab = obstacleConfig.obstaclePrefab;
            for (int i = 0; i < bonusSpawnSequenceSize; i++)
            {
                GameObject clone = Instantiate(obstaclePrefab, spawnPoint, Quaternion.identity);
                if (clone.TryGetComponent(out Obstacle obstacle))
                {
                    obstacle.ConfigureObstacle(obstacleConfig);
                }


                yield return new WaitForSeconds(1f / bonusSpawnSequenceSize);
            }

            _bonusElementsSpawning = false;
            yield return new WaitForSeconds(Mathf.Lerp(maxBonusSpawnInterval, minBonusSpawnInterval, NormalizedBonusSpawnTime));
        }
    }
    
    private IEnumerator SpawnObstacles()
    {
        while (maxPlayerLives >= 0)
        {
            float rngXPos = 0;
            do
            {
                rngXPos = Random.Range(_minSpawnCoordinate.x, _maxSpawnCoordinate.x);
            } while (Mathf.Abs(_bonusElementsXPosSpawn - rngXPos) < DistanceToKeepFromBonusElements && _bonusElementsSpawning);
            float yPos = _minSpawnCoordinate.y;

            Vector2 spawnPoint = new Vector2(rngXPos, yPos);
            ObstacleConfiguration obstacleConfig = _allObstacles.GetRandom();
            GameObject obstaclePrefab = obstacleConfig.obstaclePrefab;
            GameObject clone = Instantiate(obstaclePrefab, spawnPoint, Quaternion.identity);
            if (clone.TryGetComponent(out Obstacle obstacle))
            {
                obstacle.ConfigureObstacle(obstacleConfig);
            }

            float spawnInterval = Mathf.Lerp(maxObstacleSpawnInterval, minObstacleSpawnInterval, NormalizedObstacleSpawnTime);
            UnityEngine.Debug.Log($"Spawn Interval: {spawnInterval}");

            yield return new WaitForSeconds(spawnInterval);
        }
        yield break;
    }

    private void Update()
    {
        if (maxPlayerLives > 0)
        {
            _currentScore += Time.deltaTime * scorePerSecond;
            uiScore.UpdateScore(_currentScore);
            if (_currentGameDuration < maxGameDurationSeconds)
            {
                _currentGameDuration += Time.deltaTime;
            }
        }

    }
    
    public void ChangeScore(float amount)
    {
        _currentScore += amount;
        if (_currentScore < 0)
        {
            _currentScore = 0;
        }
        
        uiScore.UpdateScore(_currentScore, true);
        
        int sign = (int)Mathf.Sign(amount);
        if (sign < 0)
        {
            maxPlayerLives--;
            Screenshake.Instance.ShakeScreen(0.5f, 0.25f);
            _currentGameDuration *= 1 - onScoreLossSlowdownPercentage / 100;
        }
    }
}