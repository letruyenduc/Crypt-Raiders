using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    private RoomManager currentRoom;

    void Start()
    {
        currentHealth = maxHealth;
        // On récupère le manager de la salle dans laquelle l'ennemi a spawn
        currentRoom = GetComponentInParent<RoomManager>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(name + " a reçu " + damage + " dégâts. Vie restante : " + currentHealth);

        // Feedback visuel rapide (clignotement rouge)
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.1f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void Die()
    {
        Debug.Log(name + " est mort !");
        // Optionnel : Instancier du loot ici plus tard
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