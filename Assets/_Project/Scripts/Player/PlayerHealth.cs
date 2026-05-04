using UnityEngine;
using UnityEngine.UI;
using TMPro; // Requis pour le texte

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    private bool isDead = false;

    [Header("Interface (HUD)")]
    public Slider healthBar;
    public TextMeshProUGUI healthText; // Ajoute cette variable

    void Start()
    {
        currentHealth = maxHealth;
        
        if (healthBar != null) healthBar.maxValue = maxHealth;
        UpdateHealthUI(); // On centralise la mise à jour visuelle
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        
        // Empêche l'affichage de nombres négatifs (ex: -2 / 10)
        if (currentHealth < 0) currentHealth = 0; 
        
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Nouvelle fonction dédiée uniquement à l'affichage
    private void UpdateHealthUI()
    {
        if (healthBar != null) 
        {
            healthBar.value = currentHealth;
        }
        if (healthText != null) 
        {
            healthText.text = currentHealth + " / " + maxHealth;
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