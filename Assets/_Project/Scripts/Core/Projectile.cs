using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private float speed;
    private float lifetime;
    private Vector2 direction;
    private LayerMask enemyLayers;
    private LayerMask wallLayers;

    public void Setup(int damage, float speed, float lifetime, Vector2 direction, LayerMask enemyLayers, LayerMask wallLayers)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.direction = direction.normalized;
        this.enemyLayers = enemyLayers;
        this.wallLayers = wallLayers;

        // Rotation du projectile vers sa direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On vérifie si on touche un ennemi
        if (((1 << collision.gameObject.layer) & enemyLayers) != 0)
        {
            EnemyHealth health = collision.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
            return;
        }
        
        // On vérifie si on touche un mur (via le LayerMask configurable)
        if (((1 << collision.gameObject.layer) & wallLayers) != 0)
        {
            Destroy(gameObject);
        }
    }
}
