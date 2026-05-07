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
    public int baseAttackDamage = 15;
    public float attackCooldown = 0.5f; 

    [Header("Visuals")]
    public SpriteRenderer weaponSpriteRenderer;

    private int bonusPhysique = 0;
    private int bonusMagie = 0;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private Vector2 aimDirection = Vector2.down; 
    private float nextAttackTime = 0f;
    public LayerMask wallLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // S'abonner aux changements d'inventaire pour mettre à jour l'arme visuellement
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += UpdateWeaponVisuals;
            InventoryManager.Instance.UpdatePlayerStats(); // Forcer la synchro des bonus
            UpdateWeaponVisuals(); // Mise à jour initiale
        }
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= UpdateWeaponVisuals;
        }
    }

    public void UpdateWeaponVisuals()
    {
        if (weaponSpriteRenderer == null) return;

        // PAR DÉFAUT : On cache l'arme
        weaponSpriteRenderer.enabled = false;

        if (InventoryManager.Instance == null) return;

        // On vérifie si une arme est équipée
        if (InventoryManager.Instance.equipment.TryGetValue(ItemType.Arme_Melee, out ItemInstance weapon) && weapon != null)
        {
            Debug.Log("PlayerController: Affichage de l'arme équipée : " + weapon.template.itemName);
            weaponSpriteRenderer.sprite = weapon.template.itemIcon;
            weaponSpriteRenderer.enabled = true;
        }
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
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= nextAttackTime)
            {
                Debug.Log("PlayerController: Clic détecté, lancement de Attack()");
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
            else
            {
                Debug.Log("PlayerController: Attaque en cooldown...");
            }
        }
    }

    void FixedUpdate()
    {
        // Déplacement physique
        rb.velocity = moveInput * moveSpeed;
    }

    void Attack()
    {
        // Déclenche l'animation via le Trigger défini dans l'Animator
        if (animator != null)
        {
            Debug.Log("PlayerController: Envoi du Trigger 'Attack' à l'Animator");
            animator.SetTrigger("Attack");
        }
        else
        {
            Debug.LogWarning("PlayerController: Aucun Animator trouvé sur le joueur !");
            PerformDamageDetection();
        }
    }

    // Cette méthode peut être appelée par un "Animation Event" dans le clip d'attaque
    // pour que les dégâts soient synchronisés avec le mouvement de l'arme.
    public void PerformDamageDetection()
    {
        // 1. Calcul de la position du cercle d'attaque
        // On utilise l'offset de 0.8f pour projeter le cercle devant le joueur
        Vector2 attackPosition = (Vector2)transform.position + aimDirection * attackOffset;

        // 2. Détection de tous les ennemis dans le rayon de 0.6f
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // --- VÉRIFICATION DU MUR ---
            RaycastHit2D wallCheck = Physics2D.Linecast(transform.position, enemy.transform.position, wallLayer);

            if (wallCheck.collider == null)
            {
                EnemyHealth health = enemy.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    int finalDamage = baseAttackDamage + bonusPhysique;
                    health.TakeDamage(finalDamage); 
                }
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