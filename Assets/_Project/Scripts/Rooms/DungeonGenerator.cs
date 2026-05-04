using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    // Instance unique pour être accessible par toutes les salles
    public static DungeonGenerator instance;

    [Header("Configuration")]
    public GameObject[] roomPrefabs; // Tes modèles de salles de combat
    
    void Awake()
    {
        // Système Singleton : permet au RoomManager de dire "Génère une salle"
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SpawnNextRoom(Vector3 position)
    {
        if (roomPrefabs.Length == 0)
        {
            Debug.LogError("Aucun prefab de salle n'est assigné dans le DungeonGenerator !");
            return;
        }

        // Choisit une salle au hasard dans ton tableau
        int randomIndex = Random.Range(0, roomPrefabs.Length);
        
        // Crée la salle à la position donnée (le point de sortie de la salle précédente)
        Instantiate(roomPrefabs[randomIndex], position, Quaternion.identity);
        
        Debug.Log("Nouvelle salle générée à : " + position);
    }
}