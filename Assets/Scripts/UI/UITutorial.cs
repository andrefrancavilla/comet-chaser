using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
    [SerializeField] private GameObject countdownPanel;
    
    public void Play()
    {
        gameObject.SetActive(false);
        countdownPanel.SetActive(true);
    }
}
