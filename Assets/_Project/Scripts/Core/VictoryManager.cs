using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    [Header("UI Elements")]
    public GameObject victoryUI;
    public string lobbySceneName = "Lobby";

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void ShowVictory()
    {
        // On affiche le panneau
        if (victoryUI != null) victoryUI.SetActive(true);
        
        // --- NOUVEAUTÉ : GÉNÉRER LE LOOT ---
        if (LootRewardUI.Instance != null)
        {
            LootRewardUI.Instance.ShowRewards();
        }

        // On débloque la souris pour pouvoir cliquer sur les boutons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Optionnel : figer le temps si tu ne veux pas que le joueur bouge encore
        // Time.timeScale = 0f; 
    }

    // Fonction pour le bouton "Rejouer"
    public void Replay()
    {
        if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();
        Time.timeScale = 1f;
        // Recharge la scène actuelle (le donjon)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Fonction pour le bouton "Lobby"
    public void GoToLobby()
    {
        if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();
        Time.timeScale = 1f;
        SceneManager.LoadScene(lobbySceneName);
    }
}