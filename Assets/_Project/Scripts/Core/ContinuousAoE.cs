using UnityEngine;
using System.Collections;

public class ContinuousAoE : MonoBehaviour
{
    private int damage;
    private float radius;
    private float duration;
    private LayerMask enemyLayers;
    private Transform playerTransform;

    [Header("Settings")]
    public float tickRate = 0.5f; // Dégâts toutes les 0.5s

    public void Setup(int damage, float radius, float duration, LayerMask enemyLayers)
    {
        this.damage = damage;
        this.radius = radius;
        this.duration = duration;
        this.enemyLayers = enemyLayers;

        // Trouver le joueur pour le suivre
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        // Ajuster la taille visuelle (Diamètre)
        transform.localScale = new Vector3(radius * 2f, radius * 2f, 1f);

        StartCoroutine(DamageRoutine());
        Destroy(gameObject, duration);
    }

    void Update()
    {
        // Reste collé au joueur
        if (playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
    }

    private IEnumerator DamageRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            // Détection circulaire
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayers);
            foreach (Collider2D hit in hits)
            {
                EnemyHealth health = hit.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }

            yield return new WaitForSeconds(tickRate);
            elapsed += tickRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
