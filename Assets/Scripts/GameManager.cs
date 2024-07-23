using System.Collections;
using System.Collections.Generic;
using EditorUtilities.CustomAttributes;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int scorePerSecond = 1000; //1 punto ogni millesimo di secondo = 1000 punti al secondo
    [SerializeField] private float maxGameDurationSeconds = 300;
    [SerializeField] private float onScoreLossSlowdownPercentage = 20;
    [SerializeField] private List<ObstacleConfiguration> _allObstacles;
    
    [SerializeField] private UIScore uiScore;
    [ReadOnly] private float _currentScore;
    [ReadOnly] private float _currentGameDuration;

    public float NormalizedTime => Mathf.Clamp(_currentGameDuration / maxGameDurationSeconds, 0, 1);

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            ChangeScore(2000);
            yield return new WaitForSeconds(2);
            ChangeScore(-3000);
        }
    }

    private void Update()
    {
        _currentScore += Time.deltaTime * scorePerSecond;
        uiScore.UpdateScore(_currentScore);

        _currentGameDuration += Time.deltaTime;
    }
    
    public void ChangeScore(float amount)
    {
        _currentScore += amount;
        uiScore.UpdateScore(_currentScore, true);
        
        int sign = (int)Mathf.Sign(amount);
        if(sign < 0)
            Screenshake.Instance.ShakeScreen(1, 1);
    }
}