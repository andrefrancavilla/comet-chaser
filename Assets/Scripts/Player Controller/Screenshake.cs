using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Screenshake : Singleton<Screenshake>
{
    [SerializeField] private float _shakeEaseAmount;
    
    private float _currentScreenshakeIntensity;
    private float _currentScreenshakeDuration;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (_currentScreenshakeDuration <= 0)
        {
            transform.position = Vector3.Lerp(transform.position, _startPosition, Time.deltaTime / _shakeEaseAmount);
            return;
        }

        _currentScreenshakeDuration -= Time.deltaTime;

        Vector3 rngPos = (Vector2)_startPosition + Random.insideUnitCircle * _currentScreenshakeIntensity;
        rngPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, rngPos, Time.deltaTime / _shakeEaseAmount);
    }

    public void ShakeScreen(float intensity, float duration)
    {
        _currentScreenshakeIntensity = intensity;
        _currentScreenshakeDuration = duration;
    }
}
