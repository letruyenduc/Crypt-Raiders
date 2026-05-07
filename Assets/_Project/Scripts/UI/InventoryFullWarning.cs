using UnityEngine;
using TMPro;

public class InventoryFullWarning : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject warningPanel;
    public float displayDuration = 2.0f;

    void Start()
    {
        if (warningPanel != null) warningPanel.SetActive(false);

        // On s'abonne à l'événement du manager
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryFull += ShowWarning;
        }
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryFull -= ShowWarning;
        }
    }

    public void ShowWarning()
    {
        if (warningPanel == null) return;

        StopAllCoroutines();
        warningPanel.SetActive(true);
        Invoke("HideWarning", displayDuration);
    }

    private void HideWarning()
    {
        if (warningPanel != null) warningPanel.SetActive(false);
    }
}
