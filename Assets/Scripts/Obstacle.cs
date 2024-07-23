using EditorUtilities.CustomAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject onCollisionEffect;
    [SerializeField, Tooltip("Degrees per second")] private float zRotationSpeed;
    [ReadOnly, SerializeField] protected ObstacleConfiguration _obstacleConfiguration;

    private Rigidbody2D _rb;

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + zRotationSpeed * Time.deltaTime);
    }

    public void ConfigureObstacle(ObstacleConfiguration config)
    {
        _obstacleConfiguration = config;

        _rb = GetComponent<Rigidbody2D>();

        float rngSize = Random.Range(config.minObstacleSize, config.maxObstacleSize);
        transform.localScale = Vector3.one * rngSize;

        float distanceFromPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        float travelDuration = Mathf.Lerp(config.minObstacleFallDuration, config.maxObstacleFallDuration, config.progressionCurve.Evaluate(GameManager.Instance.NormalizedMaxTime));

        //Apply % variation
        float variation = travelDuration * (config.fallDurationVariationPercentage / 2 / 100);
        variation = Random.Range(-variation, variation);
        travelDuration += variation;
        
        float yVel = distanceFromPlayer / travelDuration;
        _rb.velocity = new Vector2(0, -yVel);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        GameManager.Instance.ChangeScore(GetScoreVariation());

        if (onCollisionEffect != null)
        {
            Instantiate(onCollisionEffect, transform.position, transform.rotation);
        }
    }

    public virtual float GetScoreVariation() => _obstacleConfiguration.maxScoreVariation;
}