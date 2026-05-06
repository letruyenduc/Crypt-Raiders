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
        healthBar.maxValue = boss.maxHealth;
        healthBar.value = boss.maxHealth;
    }

    public void UpdateBossBar(int currentHealth)
    {
        healthBar.value = currentHealth;
        
        if (currentHealth <= 0)
        {
            bossPanel.SetActive(false);
        }
    }
}
