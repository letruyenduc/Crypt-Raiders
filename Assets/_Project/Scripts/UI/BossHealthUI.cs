using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthUI : MonoBehaviour
{
    public static BossHealthUI Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject bossPanel;
    public Slider healthBar;
    public TextMeshProUGUI bossNameText;
    public TextMeshProUGUI healthText; // Nouveau : Pour afficher "500 / 500"

    private int currentBossMaxHealth;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        bossPanel.SetActive(false);
    }

    public void ShowBossBar(EnemyHealth boss)
    {
        bossPanel.SetActive(true);
        bossNameText.text = boss.bossName;
        currentBossMaxHealth = boss.maxHealth;
        
        healthBar.maxValue = currentBossMaxHealth;
        healthBar.value = currentBossMaxHealth;

        if (healthText != null)
        {
            healthText.text = currentBossMaxHealth + " / " + currentBossMaxHealth;
        }
    }

    public void UpdateBossBar(int currentHealth)
    {
        healthBar.value = currentHealth;

        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + currentBossMaxHealth;
        }
        
        if (currentHealth <= 0)
        {
            bossPanel.SetActive(false);
        }
    }
}
