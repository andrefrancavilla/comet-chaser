using System;
using System.Collections;
using System.Collections.Generic;
using EditorUtilities.CustomAttributes;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("External References")]
    [SerializeField] private UIScore uiScore;
    [Header("Game Configuration")]
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

    private void Awake()
    {
        _cam = Camera.main;
        _minSpawnCoordinate = _cam.ScreenToWorldPoint(new Vector3(0, 0));
        _maxSpawnCoordinate = _cam.ScreenToWorldPoint(new Vector3(Screen.currentResolution.width, 0));

        //Increase offset by 1 to ensure items spawn off-screen
        _minSpawnCoordinate.y += 1;
        _maxSpawnCoordinate.y += 1;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            ChangeScore(2000);
            yield return new WaitForSeconds(2);
            ChangeScore(-3000);
        }
    }

    private void Update()
    {
        _currentScore += Time.deltaTime * scorePerSecond;
        uiScore.UpdateScore(_currentScore);

        if (_currentGameDuration < maxGameDurationSeconds)
        {
            _currentGameDuration += Time.deltaTime;
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
            Screenshake.Instance.ShakeScreen(0.5f, 0.25f);
            _currentGameDuration *= 1 - onScoreLossSlowdownPercentage / 100;
        }
    }
}