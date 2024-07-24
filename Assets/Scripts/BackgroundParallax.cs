using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private float startYScrollVelocity = 0.2f;
    [SerializeField] private float maxYScrollVelocity;
    
    private SpriteRenderer _rend;
    private Material _mat;
    private float _currYScroll;


    // Start is called before the first frame update
    void Start()
    {
        _rend = GetComponent<SpriteRenderer>();
        _mat = _rend.material;
    }

    // Update is called once per frame
    void Update()
    {
        _currYScroll += Mathf.Lerp(startYScrollVelocity, maxYScrollVelocity, GameManager.Instance.NormalizedMaxTime) * Time.deltaTime;
        
        _mat.SetTextureOffset("_MainTex", new Vector2(0, _currYScroll));
    }
}
