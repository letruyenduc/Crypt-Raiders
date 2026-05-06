using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    public CanvasGroup gameOverUI;
    public float fadeDuration = 2f; 
    public string lobbySceneName = "Lobby"; 

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void TriggerGameOver()
    {
        // 1. ON GÈLE LE JEU IMMÉDIATEMENT
        // Les ennemis, la physique et les animations liées au temps s'arrêtent net.
        Time.timeScale = 0f; 

        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            // 2. ON UTILISE LE TEMPS RÉEL (Unscaled)
            // Cela permet à l'UI de s'animer même si le jeu est en pause
            elapsedTime += Time.unscaledDeltaTime; 
            
            gameOverUI.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null; 
        }

        gameOverUI.alpha = 1f;

        // 3. PAUSE EN TEMPS RÉEL
        // WaitForSeconds normal ne marcherait pas ici à cause du timeScale à 0
        yield return new WaitForSecondsRealtime(1f); 

        // 4. SÉCURITÉ ABSOLUE : DÉGELER LE TEMPS
        // Si tu oublies ça, ta scène Lobby sera complètement figée à son chargement !
        Time.timeScale = 1f;

        // --- SAUVEGARDE OU SUPPRESSION (Hardcore) ---
        if (DifficultyManager.Instance != null && DifficultyManager.Instance.currentDifficulty == DifficultyLevel.Cauchemar)
        {
            if (SaveManager.Instance != null) SaveManager.Instance.ClearSave();
            Debug.Log("GAME OVER: Mode Cauchemar détecté. Sauvegarde supprimée !");
        }
        else
        {
            if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();
        }

        SceneManager.LoadScene(lobbySceneName);
        }}