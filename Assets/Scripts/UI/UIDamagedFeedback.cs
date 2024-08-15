using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDamagedFeedback : MonoBehaviour
{
    [SerializeField] private AnimationCurve damageFadeCurve;
    [SerializeField] private float damageFadeDuration;
    [SerializeField] private Image damageImg;

    private float _tEval;
    
    // Start is called before the first frame update
    void Start()
    {
        damageImg = GetComponent<Image>();
        GameManager.Instance.onPlayerDamaged += OnPlayerDamaged; 
    }

    private void OnPlayerDamaged(float scoreVariation)
    {
        damageImg.color = Color.white;
        _tEval = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_tEval > 0)
        {
            Color c = damageImg.color;
            _tEval -= Time.deltaTime / damageFadeDuration;
            c.a = damageFadeCurve.Evaluate(_tEval);
            damageImg.color = c;
        }
    }
}
