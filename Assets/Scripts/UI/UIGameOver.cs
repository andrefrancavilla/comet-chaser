using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private GameObject headsUpDisplay;
    [SerializeField] private GameObject postGamePanel;
    [SerializeField] private float endgameDurationSeconds = 2;
    
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

        StartCoroutine(ShowPostGame());
    }

    private IEnumerator ShowPostGame()
    {
        yield return new WaitForSeconds(endgameDurationSeconds);
        postGamePanel.SetActive(true);
    }
    
    private void OnDestroy()
    {
        if(GameManager.Instance == null) return;
        GameManager.Instance.onGameOver -= OnGameOver;
    }
}
