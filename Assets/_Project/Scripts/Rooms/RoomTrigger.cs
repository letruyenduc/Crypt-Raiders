using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // On ne déclenche qu'une seule fois et uniquement pour le joueur
        if (!hasTriggered && other.CompareTag("Player"))
        {
            // On cherche le RoomManager sur le parent
            RoomManager roomManager = GetComponentInParent<RoomManager>();
            
            if (roomManager != null)
            {
                hasTriggered = true;
                
                // --- NOUVEAUTÉ : ON DÉFINIT LE SPAWN ICI PILE ---
                if (RespawnManager.Instance != null)
                {
                    RespawnManager.Instance.UpdateSafePosition(transform.position);
                }

                roomManager.OnPlayerEnter();
                Debug.Log("RoomTrigger: Joueur détecté. Activation de la salle et spawn mis à jour.");
            }
            else
            {
                Debug.LogError("RoomTrigger: Aucun RoomManager trouvé dans les parents !");
            }
        }
    }
}
