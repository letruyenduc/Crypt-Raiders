using UnityEngine;
using System.Collections.Generic;

public class LootRewardUI : MonoBehaviour
{
    public static LootRewardUI Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject rewardPanel;
    public InventorySlotUI[] rewardSlots; // Doit être de taille 3

    private List<ItemInstance> currentRewards;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        rewardPanel.SetActive(false);
    }

    public void ShowRewards()
    {
        int lootCount = 3; // Par défaut Normal

        if (DifficultyManager.Instance != null)
        {
            switch (DifficultyManager.Instance.currentDifficulty)
            {
                case DifficultyLevel.Facile: lootCount = 1; break;
                case DifficultyLevel.Normal: lootCount = 3; break;
                case DifficultyLevel.Difficile: lootCount = 4; break;
                case DifficultyLevel.Cauchemar: lootCount = 5; break;
            }
        }

        currentRewards = LootManager.Instance.GenerateRewards(lootCount);

        // --- NOUVEAUTÉ : AJOUT INSTANTANÉ ---
        // On donne les objets dès qu'ils sont générés
        foreach (var item in currentRewards)
        {
            LootManager.Instance.GiveLootToPlayer(item);
        }

        rewardPanel.SetActive(true);
        
        // On cache tout au début
        foreach (var slot in rewardSlots) 
        {
            slot.gameObject.SetActive(false);
            CanvasGroup cg = slot.GetComponent<CanvasGroup>();
            if (cg == null) cg = slot.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
        }

        StartCoroutine(LootSequence());
    }

    private System.Collections.IEnumerator LootSequence()
    {
        // 1. Apparition 1 par 1
        for (int i = 0; i < currentRewards.Count; i++)
        {
            if (i >= rewardSlots.Length) break;

            rewardSlots[i].gameObject.SetActive(true);
            rewardSlots[i].SetItem(currentRewards[i]);
            
            // Fade In
            yield return StartCoroutine(FadeSlot(rewardSlots[i], 0f, 1f, 0.5f));
            yield return new WaitForSeconds(0.3f); // Petit délai entre chaque
        }

        // 2. Pause pour laisser admirer le loot
        yield return new WaitForSeconds(2.0f);

        // 3. Fade Out global
        yield return StartCoroutine(FadePanel(rewardPanel, 1f, 0f, 0.8f));
        
        rewardPanel.SetActive(false);
    }

    private System.Collections.IEnumerator FadeSlot(InventorySlotUI slot, float start, float end, float duration)
    {
        CanvasGroup cg = slot.GetComponent<CanvasGroup>();
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        cg.alpha = end;
    }

    private System.Collections.IEnumerator FadePanel(GameObject panel, float start, float end, float duration)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();
        
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        cg.alpha = end;
    }

    // On supprime les anciennes fonctions de boutons
}
