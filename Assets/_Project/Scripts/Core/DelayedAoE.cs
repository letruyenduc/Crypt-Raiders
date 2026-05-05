using UnityEngine;
using System.Collections;

public class DelayedAoE : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject warningVisual; // Le cercle qui s'affiche avant
    public GameObject impactVisual;  // L'explosion de pierre

    private int damage;
    private float delay;
    private float radius;
    private LayerMask enemyLayers;

    public void Setup(int damage, float delay, float radius, LayerMask enemyLayers)
    {
        this.damage = damage;
        this.delay = delay;
        this.radius = radius;
        this.enemyLayers = enemyLayers;

        // Mise à l'échelle automatique des visuels
        // Un cercle de taille 1x1 avec un radius de 3 doit avoir un scale de 6 (diamètre)
        float targetScale = radius * 2f; 
        if (warningVisual != null) warningVisual.transform.localScale = new Vector3(targetScale, targetScale, 1f);
        if (impactVisual != null) impactVisual.transform.localScale = new Vector3(targetScale, targetScale, 1f);

        StartCoroutine(ExecuteAoE());
    }

    private IEnumerator ExecuteAoE()
    {
        // 1. Phase de prévention (Warning)
        if (warningVisual != null) warningVisual.SetActive(true);
        if (impactVisual != null) impactVisual.SetActive(false);

        yield return new WaitForSeconds(delay);

        // 2. Phase d'impact
        if (warningVisual != null) warningVisual.SetActive(false);
        if (impactVisual != null) impactVisual.SetActive(true);

        // Détection des ennemis
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayers);
        foreach (Collider2D hit in hits)
        {
            EnemyHealth health = hit.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        // 3. Attente courte pour voir l'effet et destruction
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // Pour voir la zone dans l'éditeur
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
