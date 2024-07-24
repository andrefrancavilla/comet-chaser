using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILivesIndicator : MonoBehaviour
{
    [SerializeField] private Color onPlayerDamagedColor = Color.red;
    [SerializeField] private Vector3 onPlayerDamagedSize = Vector3.one;
    private TextMeshProUGUI _tmPro;
    
    // Start is called before the first frame update
    void Start()
    {
        _tmPro = GetComponent<TextMeshProUGUI>();
        GameManager.Instance.onPlayerDamaged += OnPlayerDamaged;
        UpdateCounter();
    }

    private void OnPlayerDamaged(float diff)
    {
        _tmPro.color = onPlayerDamagedColor;
        transform.localScale = onPlayerDamagedSize;
        UpdateCounter();
    }

    private void Update()
    {
        _tmPro.color = Color.Lerp(_tmPro.color, Color.white,   Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime);
    }

    private void UpdateCounter()
    {
        int lives = GameManager.Instance.PlayerLivesRemaining;
        if (lives > 0)
        {
            _tmPro.text = $"{GameManager.Instance.PlayerLivesRemaining.ToString()} Lives Remaining";
        }
        else
        {
            _tmPro.text = $"Last Life Remaining";
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance == null) return;
        GameManager.Instance.onPlayerDamaged -= OnPlayerDamaged;
    }
}
