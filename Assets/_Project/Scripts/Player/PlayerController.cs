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
    public int attackDamage = 1;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down; // Par défaut regarde vers le bas

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Inputs ZQSD forcés
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;
        if (Input.GetKey(KeyCode.S)) moveY = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        moveInput = new Vector2(moveX, moveY).normalized;

        if (moveInput.sqrMagnitude > 0)
        {
            lastMoveDirection = moveInput;
        }

        // 2. Attaque avec Clic Gauche (LMB)
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        // Déplacement physique
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void Attack()
    {
        // Position de l'attaque devant le joueur
        Vector2 attackPosition = (Vector2)transform.position + lastMoveDirection * attackOffset;
        
        // Détection
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("<color=red>Cible touchée : </color>" + enemy.name);
            // Prochaine étape : enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
    }

    // Visualisation dans la scène
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 pos = (Vector2)transform.position + lastMoveDirection * attackOffset;
        Gizmos.DrawWireSphere(pos, attackRange);
    }
}