using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    [Header("Settings")]
    public float timePenalty = 30f; // On perd 30 secondes en mourant
    public Vector3 lastSafePosition;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Position initiale
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) lastSafePosition = player.transform.position;
    }

    public void UpdateSafePosition(Vector3 newPos)
    {
        lastSafePosition = newPos;
    }

    public void OnPlayerDeath(PlayerHealth playerHealth)
    {
        // 1. On vérifie si c'est le mode Cauchemar
        if (DifficultyManager.Instance != null && DifficultyManager.Instance.currentDifficulty == DifficultyLevel.Cauchemar)
        {
            Debug.Log("PERMADEATH: Retour direct au lobby.");
            if (GameOverManager.instance != null) GameOverManager.instance.TriggerGameOver();
        }
        else
        {
            // 2. On respawn avec pénalité
            Debug.Log("RESPAWN: Retour à la salle précédente.");
            
            // On déduit du temps
            if (DungeonTimer.Instance != null) DungeonTimer.Instance.DeductTime(timePenalty);

            // On repositionne le joueur
            playerHealth.transform.position = lastSafePosition;

            // On réinitialise sa vie (on peut le soigner totalement ou partiellement)
            playerHealth.RespawnHeal();
        }
    }
}
