using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UICountdown : MonoBehaviour
{
    [SerializeField] private List<string> countdownText;
    [SerializeField] private float countdownIntervalSeconds = 1;
    [SerializeField] private UnityEvent onCountdownCompleted;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        foreach (string text in countdownText)
        {
            _text.text = text;
            yield return new WaitForSeconds(countdownIntervalSeconds);
        }
        onCountdownCompleted?.Invoke();
    }
}
