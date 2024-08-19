using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    /*
     * I've opted to use Unity's old input system due to the simplicity of the game
     * Most features present in Unity's new input system would be unused
     */
    
    public bool IsInvulnerable { get; private set; }

    [SerializeField] private int blinkAmount;
    [SerializeField] private float invulnerabilityDuration;
    [SerializeField] private LayerMask playerColliderMask;
    [SerializeField] private GameObject playerExplosion;
    
    private Camera _camera;
    private bool _moving;
    
    private float _minXPos;
    private float _maxXPos;

    private bool _bonusElementsSpawning;
    private float _bonusElementsXPosSpawn;

    private Collider2D _col;
    private Renderer _rend;
    
    private void Awake()
    {
        _camera = Camera.main;
        Debug.Assert(_camera != null, nameof(_camera) + " != null");
        
        _minXPos = GameManager.Instance.lhsSpawnPosition.x;
        _maxXPos = GameManager.Instance.rhsSpawnPosition.x;
        
        GameManager.Instance.onPlayerDamaged += OnPlayerDamaged;

        _col = GetComponent<Collider2D>();
        _rend = GetComponentInChildren<Renderer>();
    }

    private void OnPlayerDamaged(float diff)
    {
        if (GameManager.Instance.PlayerLivesRemaining > 0)
        {
            StartCoroutine(BecomeInvulnerable());
        }
        else
        {
            Instantiate(playerExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private IEnumerator BecomeInvulnerable()
    {
        IsInvulnerable = true;
        Color color = _rend.material.GetColor("_Color");
        color.a = 0.3f;
        _rend.material.SetColor("_Color", color);
        for (int i = 0; i < blinkAmount; i++)
        {
            _rend.enabled = false;
            yield return new WaitForSeconds(invulnerabilityDuration / 2f / blinkAmount);
            _rend.enabled = true;
            yield return new WaitForSeconds(invulnerabilityDuration / 2f / blinkAmount);
        }

        color.a = 1f;
        _rend.material.SetColor("_Color", color);
        
        IsInvulnerable = false;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Move(Input.GetTouch(0).position);
        }
        else if (Input.GetButton("Fire1"))
        {
            Move(Input.mousePosition);
        }
        else
        {
            _moving = false;
        }
    }

    private void Move(Vector2 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue, playerColliderMask);
        if(hit.transform && hit.transform.CompareTag("Player") || _moving)
        {
            Vector3 worldPos = _camera.ScreenToWorldPoint(screenPosition);
            worldPos.x = Mathf.Clamp(worldPos.x, _minXPos, _maxXPos);
            worldPos.z = transform.position.z;
            worldPos.y = transform.position.y;
            transform.position = worldPos;
            _moving = true;
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance == null) return;
        GameManager.Instance.onPlayerDamaged -= OnPlayerDamaged;
    }
}
