using UnityEngine;
using UnityEngine.SceneManagement; // Pour recommencer si on meurt

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("<color=green>Santé Joueur : </color>" + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Game Over !");
        // Recharge la scène actuelle pour recommencer
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
} 