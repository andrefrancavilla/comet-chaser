using System;
using System.Collections;
using System.Collections.Generic;
using EditorUtilities.CustomAttributes;
using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] private Color _onScoreDecreasedColor = Color.red; 
    [SerializeField] private Color _onScoreIncreasedColor = Color.green;
    [SerializeField, Tooltip("In Seconds")] private float _colorInterpolationDuration = 0.5f;

    private Color _originalTextColor;
     
    private TextMeshProUGUI _tmPro;

    private float _prevScore;

    private void Awake()
    {
        _tmPro = GetComponent<TextMeshProUGUI>();
        _originalTextColor = _tmPro.color;
    }

    private void Update()
    {
        _tmPro.color = Color.Lerp(_tmPro.color, _originalTextColor, Time.deltaTime / _colorInterpolationDuration);
    }

    public void UpdateScore(float newScore, bool notifyChange = false)
    {
        _tmPro.text = newScore.ToString("##00");

        if (notifyChange)
        {
            float scoreDiff = newScore - _prevScore;
            ViewScoreChangedFeedback(scoreDiff);
        }
        
        
        _prevScore = newScore;
    }

    private void ViewScoreChangedFeedback(float scoreDiff)
    {
        int sign = (int)Mathf.Sign(scoreDiff);
        _tmPro.color = sign > 0 ? _onScoreIncreasedColor : _onScoreDecreasedColor;
    }
}
