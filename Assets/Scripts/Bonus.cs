using UnityEngine;

public class Bonus : Obstacle
{
    private const int RegDiv = 1;
    [SerializeField] private int _baseScore = 5;
    
    /*
     * NOTE: GDD mentions a magic number "5", I don't seem to get what that number stands for but the formula makes me understand that it's a base score
     */

    //Same as base implementation but doesn't care if player is invulnerable
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.NotifyScoreChange(GetScoreVariation());

        if (onCollisionEffect != null)
        {
            GameObject clone = Instantiate(onCollisionEffect, transform.position, transform.rotation);
            if (clone.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = _rb.velocity;
            }
        }

        Destroy(gameObject);
    }

    public override float GetScoreVariation()
    {
        Transform playerTransform = PlayerController.Instance.transform;
        float playerDistFromCenter = Mathf.Abs(transform.position.x - playerTransform.position.x);
        return _baseScore + (_obstacleConfiguration.maxScoreVariation / (RegDiv + playerDistFromCenter));
    }
}