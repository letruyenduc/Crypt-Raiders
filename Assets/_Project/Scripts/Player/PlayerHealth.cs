using UnityEngine;
using UnityEngine.UI;
using TMPro; // Requis pour le texte

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    private int bonusMaxHP = 0;
    private bool isDead = false;

    [Header("Interface (HUD)")]
    public Slider healthBar;
    public TextMeshProUGUI healthText; 

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI(); 
    }

    public void UpdateMaxHPBonus(int bonus)
    {
        int oldMax = GetTotalMaxHealth();
        bonusMaxHP = bonus;
        int newMax = GetTotalMaxHealth();

        // Si la vie max augmente, on soigne du montant gagné
        if (newMax > oldMax)
        {
            currentHealth += (newMax - oldMax);
        }
        
        // On sature si la vie dépasse le nouveau max
        if (currentHealth > newMax) currentHealth = newMax;

        UpdateHealthUI();
    }

    private int GetTotalMaxHealth() => maxHealth + bonusMaxHP;

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; 
        
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

    void Die()
    {
        isDead = true;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (GameOverManager.instance != null)
        {
            GameOverManager.instance.TriggerGameOver();
        }
    }
}