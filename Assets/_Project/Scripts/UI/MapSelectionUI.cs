using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapSelectionUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mapMenuPanel;
    
    [Header("Map List (Dynamic)")]
    public Transform listContainer;    
    public GameObject mapButtonPrefab; 

    [Header("Details Display")]
    public TextMeshProUGUI mapNameText;
    public TextMeshProUGUI mapDescText;
    public Image mapImage;
    public TextMeshProUGUI selectedDifficultyText; 
    public Button playButton;

    [Header("Data")]
    public List<MapData> availableMaps = new List<MapData>();
    
    private MapData selectedMap;

    void Start()
    {
        Debug.Log("MapSelectionUI: Initialisation au démarrage.");
        if (mapMenuPanel != null) mapMenuPanel.SetActive(false);
        
        GenerateMapList();
        
        if (availableMaps.Count > 0) 
        {
            SelectMap(availableMaps[0]);
        }
        else
        {
            Debug.LogWarning("MapSelectionUI: Aucune map n'est assignée dans la liste Available Maps !");
        }

        SetDifficultyNormal(); 
    }

    private void GenerateMapList()
    {
        if (listContainer == null || mapButtonPrefab == null)
        {
            Debug.LogError("MapSelectionUI: ListContainer ou Prefab de bouton manquant dans l'inspecteur !");
            return;
        }

        // Nettoyer l'ancienne liste
        foreach (Transform child in listContainer) Destroy(child.gameObject);

        // Créer un bouton pour chaque map
        foreach (MapData map in availableMaps)
        {
            if (map == null) continue;

            GameObject btnObj = Instantiate(mapButtonPrefab, listContainer);
            btnObj.name = "Button_" + map.mapName;
            
            TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null) txt.text = map.mapName;
            
            // Action au clic
            btnObj.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log("MapSelectionUI: Clic sur le bouton de map : " + map.mapName);
                SelectMap(map);
            });
        }
    }

    public void OpenMenu()
    {
        Debug.Log("MapSelectionUI: Ouverture du menu.");
        if (mapMenuPanel != null) mapMenuPanel.SetActive(true);
        GenerateMapList(); // On s'assure que la liste est à jour
        RefreshDetails();  // On s'assure que les infos sont à jour
    }

    public void CloseMenu()
    {
        if (mapMenuPanel != null) mapMenuPanel.SetActive(false);
    }

    public void SelectMap(MapData map)
    {
        if (map == null) return;
        selectedMap = map;
        Debug.Log("MapSelectionUI: Map sélectionnée : " + map.mapName);
        RefreshDetails();
        
        if (playButton != null) playButton.interactable = true;
    }

    private void RefreshDetails()
    {
        if (selectedMap == null) 
        {
            Debug.LogWarning("MapSelectionUI: RefreshDetails appelé mais aucune map sélectionnée.");
            return;
        }

        if (mapNameText != null) mapNameText.text = selectedMap.mapName;
        if (mapImage != null) mapImage.sprite = selectedMap.mapThumbnail;

        // Calcul du niveau recommandé
        int baseLevel = selectedMap.recommendedLevel;
        int adjustedLevel = baseLevel;

        if (DifficultyManager.Instance != null)
        {
            switch (DifficultyManager.Instance.currentDifficulty)
            {
                case DifficultyLevel.Facile: adjustedLevel = baseLevel; break;
                case DifficultyLevel.Normal: adjustedLevel = baseLevel; break;
                case DifficultyLevel.Difficile: adjustedLevel = baseLevel + 10; break;
                case DifficultyLevel.Cauchemar: adjustedLevel = baseLevel + 30; break;
            }
        }

        if (mapDescText != null)
        {
            mapDescText.text = $"{selectedMap.mapDescription}\n\n<color=yellow>Niveau Recommandé : {adjustedLevel}</color>";
            Debug.Log("MapSelectionUI: Détails mis à jour pour " + selectedMap.mapName);
        }
        else
        {
            Debug.LogError("MapSelectionUI: La case Map Desc Text est VIDE dans l'inspecteur !");
        }
    }

    public void StartDungeon()
    {
        if (selectedMap != null)
        {
            Debug.Log($"MapSelectionUI: Lancement du donjon : {selectedMap.sceneName}");
            if (SaveManager.Instance != null) SaveManager.Instance.SaveGame();
            Time.timeScale = 1f;
            SceneManager.LoadScene(selectedMap.sceneName);
        }
    }

    // --- SÉLECTION DE DIFFICULTÉ ---
    public void SetDifficultyFacile() { SetDifficulty(DifficultyLevel.Facile, "Facile (Butin réduit)"); }
    public void SetDifficultyNormal() { SetDifficulty(DifficultyLevel.Normal, "Normal"); }
    public void SetDifficultyDifficile() { SetDifficulty(DifficultyLevel.Difficile, "Difficile (Butin x2)"); }
    public void SetDifficultyCauchemar() { SetDifficulty(DifficultyLevel.Cauchemar, "CAUCHEMAR (Butin x5 - UNE SEULE VIE)"); }

    private void SetDifficulty(DifficultyLevel level, string label)
    {
        Debug.Log("MapSelectionUI: Tentative de changement de difficulté vers : " + label);
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.currentDifficulty = level;
            if (selectedDifficultyText != null) 
            {
                selectedDifficultyText.text = "Difficulté : " + label;
                // Si Cauchemar, on met le texte en rouge clignotant ou juste rouge pour prévenir
                if(level == DifficultyLevel.Cauchemar) selectedDifficultyText.text = $"<color=red>{selectedDifficultyText.text}</color>";
            }
            RefreshDetails();
        }
        else
        {
            Debug.LogError("MapSelectionUI: DifficultyManager INTROUVABLE !");
        }
    }
}
