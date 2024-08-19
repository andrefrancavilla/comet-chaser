using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIScoreStarsLerp : MonoBehaviour
{
    [SerializeField] private float starsPerScore = 0.25f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject star;
    [SerializeField] private float spawnRadius = 3.14f;
    [SerializeField] private float minScale = 0.7f;
    [SerializeField] private float maxScale = 1f;
    
    private bool _moveAllowed;

    public void SpawnStars(float score)
    {
        int starCount = (int)(starsPerScore * Mathf.Abs(score));
        for (int i = 0; i < starCount; i++)
        {
            Vector2 rngSpawnPoint = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
            var clone = Instantiate(star, rngSpawnPoint, Quaternion.Euler(0, 0, Random.Range(-33f, 33f)), transform);
            clone.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
        }
    }
    
    public void AllowMove()
    {
        _moveAllowed = true;
    }

    private void Update()
    {
        if(!_moveAllowed) return;

        foreach (Transform child in transform)
        {
            Vector3 moveDir = UIScore.Instance.transform.position - child.position;
            Vector3 moveDirNormalized = moveDir.normalized;

            child.transform.position += moveDirNormalized * (moveSpeed * Time.deltaTime);
            if (Vector3.Distance(child.position, UIScore.Instance.transform.position) < 10f)
            {
                Destroy(child.gameObject);
                break;
            }
        }

        if (transform.childCount == 0)
            Destroy(gameObject);
    }
}
