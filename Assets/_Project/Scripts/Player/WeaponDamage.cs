using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    private PlayerController player;
    private LayerMask enemyLayers;
    private LayerMask wallLayer;

    void Start()
    {
        // On récupère le script PlayerController sur la racine du joueur
        player = GetComponentInParent<PlayerController>();
        
        if (player != null)
        {
            enemyLayers = player.enemyLayers;
            wallLayer = player.wallLayer;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On vérifie si l'objet touché est un ennemi
        if (((1 << collision.gameObject.layer) & enemyLayers) != 0)
        {
            // Vérification des murs entre le joueur et l'ennemi (pour éviter de taper à travers)
            RaycastHit2D wallCheck = Physics2D.Linecast(player.transform.position, collision.transform.position, wallLayer);
            
            if (wallCheck.collider == null)
            {
                EnemyHealth health = collision.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    // On récupère les dégâts calculés par le PlayerController
                    // Note: Il faudrait exposer finalDamage ou recalculer ici
                    int damage = player.baseAttackDamage; // Simplification pour cet exemple
                    health.TakeDamage(damage);
                }
            }
        }
    }
}
