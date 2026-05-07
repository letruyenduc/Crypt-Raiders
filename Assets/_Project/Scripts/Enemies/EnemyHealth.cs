using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    private RoomManager currentRoom;

    [Header("UI")]
    public bool isBoss = false;
    public string bossName = "Général de Pierre";
    public UnityEngine.UI.Slider healthBar; // Barre au-dessus de sa tête
    
    // --- AJOUT : Cache de la caméra ---
    private Camera mainCamera; 

    [Header("Rewards")]
    public int xpReward = 50;
    public int goldReward = 20;

    void Start()
    {
        // --- NOUVEAUTÉ : DIFFICULTÉ ---
        if (DifficultyManager.Instance != null)
        {
            maxHealth = Mathf.RoundToInt(maxHealth * DifficultyManager.Instance.GetHealthMultiplier());
            xpReward = Mathf.RoundToInt(xpReward * DifficultyManager.Instance.GetRewardMultiplier());
            goldReward = Mathf.RoundToInt(goldReward * DifficultyManager.Instance.GetRewardMultiplier());
        }

        currentHealth = maxHealth;
        
        mainCamera = Camera.main;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // Si c'est un boss, on prévient le HUD global
        if (isBoss && BossHealthUI.Instance != null)
        {
            BossHealthUI.Instance.ShowBossBar(this);
        }

        currentRoom = GetComponentInParent<RoomManager>();
    }

    [Header("Visual Effects")]
    public GameObject floatingTextPrefab;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Mise à jour de la petite barre au-dessus de la tête
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // --- NOUVEAUTÉ : MISE À JOUR DE LA GROSSE BARRE DE BOSS ---
        if (isBoss && BossHealthUI.Instance != null)
        {
            BossHealthUI.Instance.UpdateBossBar(currentHealth);
        }

        // --- TEXTE FLOTTANT ---
        if (floatingTextPrefab != null)
        {
            GameObject ftObj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            ftObj.GetComponent<FloatingText>().Setup(damage);
        }

        Debug.Log(name + " a reçu " + damage + " dégâts. Vie restante : " + currentHealth);

        // Feedback visuel rapide (clignotement rouge)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
            Invoke("ResetColor", 0.1f);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ResetColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.white;
    }

    void Die()
    {
        Debug.Log(name + " est mort !");
        
        // Donner les récompenses au joueur
        if (LevelManager.Instance != null) LevelManager.Instance.AddXP(xpReward);
        if (CurrencyManager.Instance != null) CurrencyManager.Instance.AddGold(goldReward);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // C'est ici que la salle vérifie si elle est vide
        if (currentRoom != null)
        {
            currentRoom.CheckEnemies();
        }
    }
}
