using UnityEngine;
using UnityEngine.UI;
using TMPro; // Requis pour le texte

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private int bonusMaxHP = 0;
    private bool isDead = false;
    private bool isInvincible = false; // Nouveau

    [Header("Interface (HUD)")]
    public Slider healthBar;
    public TextMeshProUGUI healthText; 

    [Header("Respawn Settings")]
    public float invincibilityDuration = 2f;

    void Start()
    {
        // On demande à l'InventoryManager de nous envoyer les bonus actuels
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.UpdatePlayerStats();
        }

        // --- NOUVEAUTÉ : SOIN TOTAL AU CHARGEMENT ---
        // On remet la vie au maximum (incluant les bonus d'armure/stats)
        currentHealth = GetTotalMaxHealth();
        UpdateHealthUI(); 
    }

    public void UpdateMaxHPBonus(int bonus)
    {
        float healthPercent = (float)currentHealth / GetTotalMaxHealth();
        
        bonusMaxHP = bonus;
        int newMax = GetTotalMaxHealth();

        // On maintient le même pourcentage de vie après le changement d'équipement
        currentHealth = Mathf.RoundToInt(newMax * healthPercent);
        
        // Sécurité : si on était full vie, on reste au max
        if (healthPercent >= 0.99f) currentHealth = newMax;
        // Sécurité : ne pas mourir en enlevant une armure
        if (currentHealth <= 0 && newMax > 0) currentHealth = 1;

        UpdateHealthUI();
    }

    private int GetTotalMaxHealth() => maxHealth + bonusMaxHP;

    public void TakeDamage(int damage)
    {
        if (isDead || isInvincible) return; // Ignore si mort ou invincible

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; 
        
        // --- NOUVEAUTÉ : FLASH ROUGE ---
        if (DamageFlashUI.Instance != null) DamageFlashUI.Instance.TriggerFlash();

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        int totalMax = GetTotalMaxHealth();
        if (healthBar != null) 
        {
            healthBar.maxValue = totalMax;
            healthBar.value = currentHealth;
        }
        if (healthText != null) 
        {
            healthText.text = currentHealth + " / " + totalMax;
        }
    }

    public void RespawnHeal()
    {
        isDead = false;
        currentHealth = GetTotalMaxHealth();
        UpdateHealthUI();
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        // Lancer l'invincibilité
        StartCoroutine(InvincibilityRoutine());
    }

    private System.Collections.IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            if (sr != null) sr.enabled = !sr.enabled; // Fait clignoter
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }
        if (sr != null) sr.enabled = true;
        isInvincible = false;
    }

    void Die()
    {
        isDead = true;
        
        // --- NOUVEAUTÉ : RESPAWN DYNAMIQUE ---
        if (RespawnManager.Instance != null)
        {
            RespawnManager.Instance.OnPlayerDeath(this);
        }
        else
        {
            // Fallback si le manager est absent
            if (GameOverManager.instance != null) GameOverManager.instance.TriggerGameOver();
        }
    }
}