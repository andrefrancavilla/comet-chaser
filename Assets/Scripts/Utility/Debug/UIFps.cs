using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFps : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = $"{fps:0} FPS";
    }
}
