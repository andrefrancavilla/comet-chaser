using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class VibrationConfig
{
    [SerializeField] private VibrationEvent vibrationEvent;
    [Space, Header("Android Only")]
    [SerializeField] private bool useVibrationPattern;
    [SerializeField] private float vibrationLength;
    [SerializeField] private float[] vibrationPattern;

    [Space, Header("iOS Only")] 
    [SerializeField] private ImpactFeedbackStyle iOSFeedbackStile;

    public VibrationEvent EventType => vibrationEvent;
        
    public bool UseVibrationPattern => useVibrationPattern;
    public long VibrationLength => (long)vibrationLength;
    public long[] VibrationPattern => vibrationPattern.Select(x => (long)x).ToArray();
    public ImpactFeedbackStyle ImpactFeedbackStyle => iOSFeedbackStile;
}

[Serializable]
public enum VibrationEvent
{
    OnPlayerDamaged,
    OnBonusCollected
}