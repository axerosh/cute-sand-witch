using UnityEngine;

public class WallCannon : CastleComponent
{
    public CannonBall cannonBallPrefab;

    public Transform cannonBallSpawnPoint;

    public float cooldown;

    public float cannonBallVelocity;

    public float stormTrooperFactor;

    private float currentCooldown = 0;

    private void Update()
    {
        if (currentCooldown <= 0)
        {
            FireCannonBall();
            currentCooldown = cooldown;
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    private void FireCannonBall()
    {
        Vector2 offset = Random.insideUnitCircle * stormTrooperFactor;

        Vector3 offsetPoint = cannonBallSpawnPoint.transform.position + cannonBallSpawnPoint.transform.forward;

        offsetPoint.x += offset.x;
        offsetPoint.y += offset.y;

        CannonBall newBall = Instantiate(cannonBallPrefab, cannonBallSpawnPoint.transform.position, transform.rotation);
        newBall.SetInitialAim((offsetPoint - cannonBallSpawnPoint.transform.position).normalized * cannonBallVelocity);
    }
}
