using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationController : MonoBehaviour
{
    [SerializeField] public List<VibrationConfig> vibrationConfigs;
    private MobileDevice _currentDevice;
    
    private void Awake()
    {
        Vibration.Init();

        #if UNITY_ANDROID
        _currentDevice = MobileDevice.Android;
        #endif
        #if UNITY_IOS
        _currentDevice = MobileDevice.iOS;
        #endif
    }
    
    public void Vibrate(VibrationEvent vibEvent)
    {
        
        VibrationConfig cfg = vibrationConfigs.Find(it => it.EventType == (VibrationEvent)vibEvent);
        if (cfg == null)
        {
            Debug.LogError($"Configuration for event {vibEvent} not found.");
            return;
        }

        switch (_currentDevice)
        {
            case MobileDevice.Android:
                if (cfg.UseVibrationPattern)
                {
                    Vibration.VibrateAndroid(cfg.VibrationPattern);
                }
                else
                {
                    Vibration.VibrateAndroid(cfg.VibrationLength);
                }
                break;
            case MobileDevice.iOS:
                Vibration.VibrateIOS(cfg.ImpactFeedbackStyle);
                break;
            default:
                Debug.LogError($"Device {_currentDevice} is not supported.");
                break;
        }
    }

    public enum MobileDevice
    {
        Android,
        iOS
    }
}