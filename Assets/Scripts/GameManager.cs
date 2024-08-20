using System;
using System.Collections;
using System.Collections.Generic;
using EditorUtilities.CustomAttributes;
using InstantTools.PoolSystem;
using UnityEngine;
using UnityEngine.Events;
using Utility;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    private const float DistanceToKeepFromBonusElements = 0.75f;
    public Action<float> onPlayerDamaged;
    public Action<float> onBonusCollected;
    public Action onGameOver;

    public BetterEvent onPlayerDamagedEvent;
    public BetterEvent onBonusCollectedEvent;
    public BetterEvent onGameOverEvent;
    
    [Header("Game Configuration")] 
    [SerializeField] private Transform lhsSpawnPoint;
    [SerializeField] private Transform rhsSpawnPoint;
    [SerializeField] private int playerLives = 3;
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

    public int PlayerLivesRemaining => playerLives;

    public float CurrentScore => _currentScore;

    public Vector2 lhsSpawnPosition => lhsSpawnPoint.position;
    public Vector2 rhsSpawnPosition => rhsSpawnPoint.position;

    private Vector2 _minSpawnCoordinate;
    private Vector2 _maxSpawnCoordinate;
    private Camera _cam;

    private bool _bonusElementsSpawning;
    private float _bonusElementsXPosSpawn;
    private bool _gameStarted;

    private void Awake()
    {
        _cam = Camera.main;
        Debug.Assert(_cam != null, nameof(_cam) + " != null");
        _minSpawnCoordinate = lhsSpawnPoint.position;
        _maxSpawnCoordinate = rhsSpawnPoint.position;

        //Increase offset by 1 to ensure items spawn off-screen
        _minSpawnCoordinate.y += 1;
        _maxSpawnCoordinate.y += 1;

        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        StartCoroutine(SpawnObstacles());
        StartCoroutine(SpawnBonusElements());
        _gameStarted = true;

        UIScore.Instance.onApplyScore += ApplyScoreChange;
    }

    private IEnumerator SpawnBonusElements()
    {
        while (playerLives > 0)
        {
            _bonusElementsSpawning = true;
            float rngXPos = Random.Range(_minSpawnCoordinate.x, _maxSpawnCoordinate.x);
            float yPos = _minSpawnCoordinate.y;
            
            _bonusElementsXPosSpawn = rngXPos;

            Vector2 spawnPoint = new Vector2(rngXPos, yPos);
            ObstacleConfiguration obstacleConfig = _allBonuses.GetRandom();
            for (int i = 0; i < bonusSpawnSequenceSize; i++)
            {
                GameObject clone = PoolManager.ActivateObject(obstacleConfig.obstaclePoolName, spawnPoint, Quaternion.identity);
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
        while (playerLives > 0)
        {
            float rngXPos = 0;
            do
            {
                rngXPos = Random.Range(_minSpawnCoordinate.x, _maxSpawnCoordinate.x);
            } while (Mathf.Abs(_bonusElementsXPosSpawn - rngXPos) < DistanceToKeepFromBonusElements && _bonusElementsSpawning);
            float yPos = _minSpawnCoordinate.y;

            Vector2 spawnPoint = new Vector2(rngXPos, yPos);
            ObstacleConfiguration obstacleConfig = _allObstacles.GetRandom();
            GameObject clone = PoolManager.ActivateObject(obstacleConfig.obstaclePoolName, spawnPoint, Quaternion.identity);
            if (clone.TryGetComponent(out Obstacle obstacle))
            {
                obstacle.ConfigureObstacle(obstacleConfig);
            }

            float spawnInterval = Mathf.Lerp(maxObstacleSpawnInterval, minObstacleSpawnInterval, NormalizedObstacleSpawnTime);
            yield return new WaitForSeconds(spawnInterval);
        }
        yield break;
    }

    private void Update()
    {
        if(!_gameStarted) return;
        
        if (playerLives > 0)
        {
            _currentScore += Time.deltaTime * scorePerSecond;
            
            if (_currentGameDuration < maxGameDurationSeconds)
            {
                _currentGameDuration += Time.deltaTime;
            }
        }

    }
    
    public void NotifyScoreChange(float amount)
    {
        int sign = (int)Mathf.Sign(amount);
        if (sign < 0)
        {
            playerLives--;
            _currentGameDuration *= 1 - onScoreLossSlowdownPercentage / 100;

            if (playerLives <= 0)
            {
                onGameOver?.Invoke();
                onGameOverEvent?.Invoke();
            }
            
            onPlayerDamaged?.Invoke(amount);
            onPlayerDamagedEvent?.Invoke();
        }
        else
        {
            onBonusCollected?.Invoke(amount);
            onBonusCollectedEvent?.Invoke();
        }
    }

    //Method is executed when boost/nerf stars on screen reach ui score element
    private void ApplyScoreChange(float amount)
    {
        _currentScore += amount;
        if (_currentScore < 0)
        {
            _currentScore = 0;
        }
    }
}