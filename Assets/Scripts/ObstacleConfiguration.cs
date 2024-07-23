using System;
using UnityEngine;

[Serializable]
public class ObstacleConfiguration
{
    public GameObject obstaclePrefab;
    public float minObstacleSize;
    public float maxObstacleSize;
    [Space] 
    [Tooltip("In units/s")] public float minObstacleVelocity;
    [Tooltip("In units/s")] public float maxObstacleVelocity;
    [Tooltip("0 is the start of the game, 1 is the max duration of it")] public AnimationCurve progressionCurve;
}