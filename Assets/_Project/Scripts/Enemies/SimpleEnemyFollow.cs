using UnityEngine;
using System.Collections.Generic;

public class SimpleEnemyFollow : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 4f;
    public float rotationSpeed = 10f;
    public float chaseRange = 10f;
    public float attackRange = 1.2f;
    public float attackRate = 1f;
    public int baseDamage = 15;

    private Transform player;
    private Rigidbody2D rb;
    private float nextAttackTime = 0f;

    private List<Node> currentPath;
    private int currentPathIndex = 0;
    private float pathUpdateInterval = 0.2f; // Plus rapide
    private float lastPathUpdateTime = 0f;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        
        // S'assurer que la détection de collision est optimale
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > chaseRange)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // Mise à jour du chemin
        if (Time.time >= lastPathUpdateTime + pathUpdateInterval)
        {
            RequestNewPath();
            lastPathUpdateTime = Time.time;
        }

        FollowPath(distance);
    }

    void RequestNewPath()
    {
        if (Pathfinding.Instance != null)
        {
            List<Node> newPath = Pathfinding.Instance.FindPath(transform.position, player.position);
            if (newPath != null && newPath.Count > 0)
            {
                currentPath = newPath;
                currentPathIndex = 0;
            }
        }
    }

    void FollowPath(float distanceToPlayer)
    {
        if (currentPath == null || currentPathIndex >= currentPath.Count)
        {
            if (distanceToPlayer <= attackRange + 0.8f)
            {
                MoveTowards(player.position, distanceToPlayer);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
            return;
        }

        Node targetNode = currentPath[currentPathIndex];
        float distanceToNode = Vector2.Distance(transform.position, targetNode.worldPosition);

        // Distance de tolérance augmentée (0.5f au lieu de 0.2f)
        // car si le collider de l'ennemi touche le mur, il n'atteindra jamais le centre pile.
        if (distanceToNode < 0.5f) 
        {
            currentPathIndex++;
            if (currentPathIndex >= currentPath.Count)
            {
                rb.velocity = Vector2.zero;
                return;
            }
            targetNode = currentPath[currentPathIndex];
        }

        MoveTowards(targetNode.worldPosition, distanceToPlayer);
    }

    void MoveTowards(Vector2 targetPos, float distanceToPlayer)
    {
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        // Rotation visuelle lisse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (distanceToPlayer > attackRange)
        {
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    void Attack()
    {
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            int finalDamage = baseDamage;
            if (DifficultyManager.Instance != null)
            {
                finalDamage = Mathf.RoundToInt(baseDamage * DifficultyManager.Instance.GetDamageMultiplier());
            }
            ph.TakeDamage(finalDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (currentPath != null)
        {
            Gizmos.color = Color.cyan;
            for (int i = currentPathIndex; i < currentPath.Count; i++)
            {
                Gizmos.DrawCube(currentPath[i].worldPosition, Vector3.one * 0.3f);
                if (i > currentPathIndex)
                    Gizmos.DrawLine(currentPath[i-1].worldPosition, currentPath[i].worldPosition);
            }
        }
    }
}
