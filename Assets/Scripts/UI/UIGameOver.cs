using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    public GameObject headsUpDisplay;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.onGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        headsUpDisplay.SetActive(false);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if(GameManager.Instance == null) return;
        GameManager.Instance.onGameOver -= OnGameOver;
    }
}
