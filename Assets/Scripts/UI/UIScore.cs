using System;
using System.Collections;
using System.Collections.Generic;
using EditorUtilities.CustomAttributes;
using TMPro;
using UnityEngine;

public class UIScore : Singleton<UIScore>
{
    [SerializeField] private Color _onScoreDecreasedColor = Color.red; 
    [SerializeField] private Color _onScoreIncreasedColor = Color.green;

    [Space] 
    [SerializeField] private GameObject onScoreIncreasedUIEffect;
    [SerializeField] private GameObject onScoreDecreasedUIEffects;
    [Space]
    
    [SerializeField, Tooltip("In Seconds")] private float _colorInterpolationDuration = 0.5f;

    private Color _originalTextColor;
     
    private TextMeshProUGUI _tmPro;

    private float _prevScore;

    private void Awake()
    {
        _tmPro = GetComponent<TextMeshProUGUI>();
        _originalTextColor = _tmPro.color;
        
        GameManager.Instance.onPlayerDamaged += OnPlayerDamaged;
        GameManager.Instance.onBonusCollected += OnBonusCollected;
    }

    private void OnBonusCollected(float diff)
    {
        _tmPro.color = _onScoreIncreasedColor;
        
        Instantiate(onScoreIncreasedUIEffect, PlayerController.Instance.transform.position, Quaternion.identity, transform.root);
    }

    private void OnPlayerDamaged(float diff)
    {
        _tmPro.color = _onScoreDecreasedColor;

        Instantiate(onScoreDecreasedUIEffects, PlayerController.Instance.transform.position, Quaternion.identity, transform.root);
    }

    private void Update()
    {
        _tmPro.color = Color.Lerp(_tmPro.color, _originalTextColor, Time.deltaTime / _colorInterpolationDuration);

        _tmPro.text = GameManager.Instance.CurrentScore.ToString("###0");
    }

    private void OnDestroy()
    {
        if(GameManager.Instance == null) return;
        GameManager.Instance.onPlayerDamaged -= OnPlayerDamaged;
        GameManager.Instance.onBonusCollected -= OnBonusCollected;
    }
}
