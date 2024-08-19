using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPointsText : MonoBehaviour
{
    [SerializeField] private string preScoreText;
    [SerializeField] private string postScoreText;

    private TextMeshProUGUI _tmPro;
    private float _currentScore;
    
    // Start is called before the first frame update
    void Start()
    {
        _tmPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_tmPro.color.a < 0.1f) return;
        
        _currentScore = Mathf.Lerp(_currentScore, GameManager.Instance.CurrentScore, Time.deltaTime * 3);
        _tmPro.text = $"{preScoreText}{_currentScore:###0}{postScoreText}";
    }
}
