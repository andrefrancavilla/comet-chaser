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
        GameManager.Instance.ChangeScore(GetScoreVariation());

        if (onCollisionEffect != null)
        {
            Instantiate(onCollisionEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    public override float GetScoreVariation()
    {
        Transform playerTransform = PlayerController.Instance.transform;
        float playerDistFromCenter = Vector2.Distance(transform.position, playerTransform.position);
        return _baseScore + (_obstacleConfiguration.maxScoreVariation / (RegDiv + playerDistFromCenter));
    }
}