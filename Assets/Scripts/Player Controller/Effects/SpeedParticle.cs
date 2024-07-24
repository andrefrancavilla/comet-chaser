using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedParticle : MonoBehaviour
{
    private ParticleSystem _particle;
    [SerializeField] private Color particleColor = Color.white;
    
    // Start is called before the first frame update
    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        particleColor.a = Mathf.Lerp(0, 1, GameManager.Instance.NormalizedMaxTime);
        
        ParticleSystem.MainModule main = _particle.main;
        main.startColor = new ParticleSystem.MinMaxGradient(particleColor);
        //_particle.main = main;
    }
}
