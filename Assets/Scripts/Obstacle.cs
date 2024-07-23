using EditorUtilities.CustomAttributes;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [ReadOnly] private ObstacleConfiguration _obstacleConfiguration;

    private Rigidbody2D _rb;
    
    public void ConfigureObstacle(ObstacleConfiguration config)
    {
        _obstacleConfiguration = config;

        _rb = GetComponent<Rigidbody2D>();

        float rngSize = Random.Range(config.minObstacleSize, config.maxObstacleSize);
        transform.localScale = Vector3.one * rngSize;

        float vel = Mathf.Lerp(config.minObstacleVelocity, config.maxObstacleVelocity, config.progressionCurve.Evaluate(GameManager.Instance.NormalizedTime));
    }
}
