using System;
using EditorUtilities.CustomAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    [ReadOnly] protected ObstacleConfiguration _obstacleConfiguration;

    private Rigidbody2D _rb;
    
    public void ConfigureObstacle(ObstacleConfiguration config)
    {
        _obstacleConfiguration = config;

        _rb = GetComponent<Rigidbody2D>();

        float rngSize = Random.Range(config.minObstacleSize, config.maxObstacleSize);
        transform.localScale = Vector3.one * rngSize;

        float vel = Mathf.Lerp(config.minObstacleVelocity, config.maxObstacleVelocity, config.progressionCurve.Evaluate(GameManager.Instance.NormalizedTime));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player")) return;
        
        GameManager.Instance.ChangeScore(GetScoreVariation());
    }

    public virtual float GetScoreVariation() => _obstacleConfiguration.scoreVariation;
}