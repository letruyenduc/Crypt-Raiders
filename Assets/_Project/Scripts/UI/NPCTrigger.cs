using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public enum NPCType { Marchand, Forgeron }
    
    [Header("Configuration")]
    public NPCType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // On ne réagit qu'au joueur
        if (other.CompareTag("Player"))
        {
            if (LobbyNPCManager.Instance != null)
            {
                if (type == NPCType.Marchand)
                {
                    LobbyNPCManager.Instance.OpenMerchant();
                    Debug.Log("Bienvenue chez le Marchand !");
                }
                else
                {
                    LobbyNPCManager.Instance.OpenBlacksmith();
                    Debug.Log("Prêt à forger l'acier ?");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Quand le joueur s'en va
        if (other.CompareTag("Player"))
        {
            if (LobbyNPCManager.Instance != null)
            {
                LobbyNPCManager.Instance.CloseAllNPCs();
                
                // On ferme aussi l'inventaire pour libérer l'écran
                if (InventoryUI.Instance != null && InventoryUI.Instance.inventoryPanel.activeSelf)
                {
                    InventoryUI.Instance.ToggleInventory();
                }
                
                Debug.Log("À bientôt !");
            }
        }
    }
}
