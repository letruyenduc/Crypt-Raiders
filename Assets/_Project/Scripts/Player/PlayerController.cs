using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f; // Un peu plus rapide pour le feeling Dungeon Quest

    [Header("Attack Settings")]
    public float attackRange = 0.6f;
    public float attackOffset = 0.8f;
    public LayerMask enemyLayers;
    public int baseAttackDamage = 1;
    public float attackCooldown = 0.5f; 

    private int bonusPhysique = 0;
    private int bonusMagie = 0;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 aimDirection = Vector2.down; 
    private float nextAttackTime = 0f;
    public LayerMask wallLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateDamageBonus(int physique, int magie)
    {
        bonusPhysique = physique;
        bonusMagie = magie;
    }

    void Update()
    {
        // ... (Inputs restants identiques)
        // 1. Inputs ZQSD forcés
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;
        if (Input.GetKey(KeyCode.S)) moveY = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        moveInput = new Vector2(moveX, moveY).normalized;

        // 2. Visée avec la souris
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = ((Vector2)mouseWorldPos - (Vector2)transform.position).normalized;

        // Rotation visuelle du cube vers la souris
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        // Note: l'offset de -90f (ou +90f) peut être nécessaire selon l'orientation de base de votre sprite
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        // 3. Attaque avec Clic Gauche (LMB)
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void FixedUpdate()
    {
        // Déplacement physique
        rb.velocity = moveInput * moveSpeed;
    }

    void Attack()
    {
        // 1. Calcul de la position du cercle d'attaque
        // On utilise l'offset de 0.8f pour projeter le cercle devant le joueur
        Vector2 attackPosition = (Vector2)transform.position + aimDirection * attackOffset;

        // 2. Détection de tous les ennemis dans le rayon de 0.6f
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // --- NOUVEAUTÉ : VÉRIFICATION DU MUR ---
            // On trace une ligne entre le joueur et l'ennemi
            // wallLayer doit correspondre au layer de ta Tilemap "Murs"
            RaycastHit2D wallCheck = Physics2D.Linecast(transform.position, enemy.transform.position, wallLayer);

            // Si wallCheck.collider est nul, cela signifie qu'aucun mur n'est entre les deux
            if (wallCheck.collider == null)
            {
                EnemyHealth health = enemy.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    int finalDamage = baseAttackDamage + bonusPhysique;
                    health.TakeDamage(finalDamage); 
                }
            }
            else
            {
                Debug.Log("Attaque bloquée par un mur !");
            }
        }
    }

    // Visualisation dans la scène
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 pos = (Vector2)transform.position + aimDirection * attackOffset;
        Gizmos.DrawWireSphere(pos, attackRange);
    }
}