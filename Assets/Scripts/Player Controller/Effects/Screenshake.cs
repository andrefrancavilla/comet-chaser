using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Screenshake : Singleton<Screenshake>
{
    [SerializeField] private float shakeEaseAmount;

    [SerializeField] private float onPlayerDamagedShakeIntensity = 0.75f;
    [SerializeField] private float onPlayerDamagedShakeDuration = 0.25f;
    
    private float _currentScreenshakeIntensity;
    private float _currentScreenshakeDuration;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        GameManager.Instance.onPlayerDamaged += OnPlayerDamaged;
    }

    private void OnPlayerDamaged(float diff)
    {
        ShakeScreen(onPlayerDamagedShakeIntensity, onPlayerDamagedShakeDuration);
    }

    private void Update()
    {
        if (_currentScreenshakeDuration <= 0)
        {
            transform.position = Vector3.Lerp(transform.position, _startPosition, Time.deltaTime / shakeEaseAmount);
            return;
        }

        _currentScreenshakeDuration -= Time.deltaTime;

        Vector3 rngPos = (Vector2)_startPosition + Random.insideUnitCircle * _currentScreenshakeIntensity;
        rngPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, rngPos, Time.deltaTime / shakeEaseAmount);
    }

    public void ShakeScreen(float intensity, float duration)
    {
        _currentScreenshakeIntensity = intensity;
        _currentScreenshakeDuration = duration;
    }

    private void OnDestroy()
    {
        if(GameManager.Instance == null) return;
        GameManager.Instance.onPlayerDamaged -= OnPlayerDamaged;
    }
}
