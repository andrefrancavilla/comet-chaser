using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILivesIndicator : MonoBehaviour
{
    [SerializeField] private Sprite availableLife;
    [SerializeField] private Sprite consumedLife;
    [SerializeField] private GameObject uiLivesIndicator;
    private List<Image> _uiLivesIndicators;
    
    // Start is called before the first frame update
    void Start()
    {
        _uiLivesIndicators = new List<Image>();
        
        GameManager.Instance.onPlayerDamaged += OnPlayerDamaged;
        for (int i = 0; i < GameManager.Instance.PlayerLivesRemaining; i++)
        {
            GameObject clone = Instantiate(uiLivesIndicator, transform.position, transform.rotation, transform);
            if (clone.TryGetComponent(out Image spriteRenderer))
            {
                _uiLivesIndicators.Add(spriteRenderer);
            }
        }
        
        UpdateCounter();
    }

    private void OnPlayerDamaged(float diff)
    {
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        int lives = GameManager.Instance.PlayerLivesRemaining;

        for (int i = 0; i < _uiLivesIndicators.Count; i++)
        {
            _uiLivesIndicators[i].sprite = i < lives ? availableLife : consumedLife;
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance == null) return;
        GameManager.Instance.onPlayerDamaged -= OnPlayerDamaged;
    }
}
