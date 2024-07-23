using System;
using UnityEngine;

[Serializable]
public class ObstacleConfiguration
{
    public GameObject obstaclePrefab;
    public float minObstacleSize;
    public float maxObstacleSize;

    public float maxScoreVariation = -50;
    
    [Space] 
    [Tooltip("The amount of time in seconds it takes to reach the player's rocket")] public float minObstacleFallDuration;
    [Tooltip("The amount of time in seconds it takes to reach the player's rocket")] public float maxObstacleFallDuration;
    [Tooltip("% variation"), Range(0, 100)] public float fallDurationVariationPercentage = 20;
    [Tooltip("0 is the start of the game, 1 is the max duration of it")] public AnimationCurve progressionCurve;
}