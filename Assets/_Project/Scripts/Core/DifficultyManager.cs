using UnityEngine;

public enum DifficultyLevel { Facile, Normal, Difficile, Cauchemar }

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    public DifficultyLevel currentDifficulty = DifficultyLevel.Normal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // On garde la difficulté entre les scènes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetHealthMultiplier()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Facile: return 0.5f;
            case DifficultyLevel.Normal: return 1.0f;
            case DifficultyLevel.Difficile: return 2.5f;
            case DifficultyLevel.Cauchemar: return 10.0f;
            default: return 1.0f;
        }
    }

    public float GetDamageMultiplier()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Facile: return 0.5f;
            case DifficultyLevel.Normal: return 1.0f;
            case DifficultyLevel.Difficile: return 2.0f;
            case DifficultyLevel.Cauchemar: return 5.0f;
            default: return 1.0f;
        }
    }

    public float GetRewardMultiplier()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Facile: return 0.5f;
            case DifficultyLevel.Normal: return 1.0f;
            case DifficultyLevel.Difficile: return 2.0f;
            case DifficultyLevel.Cauchemar: return 5.0f;
            default: return 1.0f;
        }
    }

    // --- NOUVEAUTÉ : TAILLE DU DONJON ---
    public int GetMaxRooms()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Facile: return 3;     // Donjon très court
            case DifficultyLevel.Normal: return 5;     // Donjon standard
            case DifficultyLevel.Difficile: return 8;  // Donjon long
            case DifficultyLevel.Cauchemar: return 12; // Donjon épique
            default: return 5;
        }
    }
}
