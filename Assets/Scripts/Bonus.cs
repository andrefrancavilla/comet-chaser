using UnityEngine;

public class Bonus : Obstacle
{
    private const int RegDiv = 1;
    private const int BaseScore = 5;
    
    /*
     * NOTE: GDD mentions a magic number "5", I don't seem to get what that number stands for but the formula makes me understand that it's a base score
     */
    
    public override float GetScoreVariation()
    {
        Transform playerTransform = PlayerController.Instance.transform;
        float playerDistFromCenter = Vector2.Distance(transform.position, playerTransform.position);
        return BaseScore + (_obstacleConfiguration.scoreVariation / (RegDiv + playerDistFromCenter));
    }
}