using System;
using System.Collections;
using EditorUtilities.CustomAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Obstacle : MonoBehaviour
{
    [SerializeField] protected GameObject onCollisionEffect;
    [SerializeField, Tooltip("Degrees per second")] private float zRotationSpeed;
    [ReadOnly, SerializeField] protected ObstacleConfiguration _obstacleConfiguration;

    protected Rigidbody2D _rb;

    private IEnumerator Start()
    {
        int rngDir = (int)Mathf.Sign(Random.Range(-10, 10));
        zRotationSpeed *= rngDir;

        Renderer rend = GetComponentInChildren<Renderer>();
        yield return new WaitWhile(() => !rend.isVisible);
        yield return new WaitWhile(() => rend.isVisible);

        gameObject.SetActive(false);
    }

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
        float travelDuration = Mathf.Lerp(config.maxObstacleFallDuration, config.minObstacleFallDuration, config.progressionCurve.Evaluate(GameManager.Instance.NormalizedMaxTime));

        //Apply % variation
        float variation = travelDuration * (config.fallDurationVariationPercentage / 2 / 100);
        variation = Random.Range(-variation, variation);
        travelDuration += variation;
        
        float yVel = distanceFromPlayer / travelDuration;
        _rb.velocity = new Vector2(0, -yVel);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        if(PlayerController.Instance.IsInvulnerable)
            return;
        
        GameManager.Instance.NotifyScoreChange(GetScoreVariation());

        if (onCollisionEffect != null)
        {
            GameObject clone = Instantiate(onCollisionEffect, transform.position, transform.rotation);
            if (clone.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = _rb.velocity;
            }
            clone.transform.localScale = transform.localScale;
        }

        gameObject.SetActive(false);
    }

    public virtual float GetScoreVariation() => _obstacleConfiguration.maxScoreVariation;
}