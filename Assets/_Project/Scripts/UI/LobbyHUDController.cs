using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyHUDController : MonoBehaviour
{
    [Header("UI Groups")]
    [Tooltip("Groupe d'UI pour le combat (Barre de vie, sorts, etc.)")]
    public GameObject combatHUD;
    
    [Tooltip("Groupe d'UI pour le Lobby (Barre d'or, bouton de map, etc.)")]
    public GameObject lobbyHUD;

    void Start()
    {
        RefreshHUD();
    }

    // On s'abonne aussi au changement de scène au cas où le Canvas soit persistant
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshHUD();
    }

    public void RefreshHUD()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        bool isLobby = currentScene == "Lobby";

        if (isLobby)
        {
            if (combatHUD != null) combatHUD.SetActive(false);
            if (lobbyHUD != null) lobbyHUD.SetActive(true);
        }
        else
        {
            if (combatHUD != null) combatHUD.SetActive(true);
            if (lobbyHUD != null) lobbyHUD.SetActive(false);
        }
    }
}
