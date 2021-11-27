using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public int impactDamage;

    public float timeToLive;

    public Rigidbody rBody;

    private float lifeTime = 0f;

    private void Update()
    {
        lifeTime += Time.deltaTime;

        if (lifeTime > timeToLive)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            CastleComponent hitComponent = collision.rigidbody.gameObject.GetComponent<CastleComponent>();
            if ( hitComponent != null )
            {
                hitComponent.Damage(impactDamage);
                Destroy(gameObject);
            }

        }
    }

    public void SetInitialAim(Vector3 direction)
    {
        rBody.velocity = direction;
    }
}
