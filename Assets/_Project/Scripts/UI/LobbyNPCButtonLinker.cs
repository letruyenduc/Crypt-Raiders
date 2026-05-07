using UnityEngine;
using UnityEngine.UI;

public class LobbyNPCButtonLinker : MonoBehaviour
{
    public enum LobbyAction { UpgradeItem, SellItem, CloseAll }
    public LobbyAction action;

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn == null) return;

        // Nettoyage
        btn.onClick.RemoveAllListeners();

        // On cherche le manager de la scène
        LobbyNPCManager manager = FindObjectOfType<LobbyNPCManager>();

        if (manager != null)
        {
            switch (action)
            {
                case LobbyAction.UpgradeItem:
                    btn.onClick.AddListener(manager.ConfirmUpgrade);
                    break;
                case LobbyAction.CloseAll:
                    btn.onClick.AddListener(manager.CloseAllNPCs);
                    break;
            }
            Debug.Log($"LobbyNPCButtonLinker: Bouton {action} rebranché avec succès !");
        }
        else
        {
            Debug.LogWarning("LobbyNPCButtonLinker: Aucun LobbyNPCManager trouvé dans cette scène.");
        }
    }
}
