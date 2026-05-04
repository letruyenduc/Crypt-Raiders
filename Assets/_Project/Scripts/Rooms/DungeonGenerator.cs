using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    // Instance unique pour être accessible par toutes les salles
    public static DungeonGenerator instance;

    [Header("Configuration du Donjon")]
    public int maxRooms = 5; // Le nombre de salles avant le boss
    private int currentRoomCount = 0; // Le compteur actuel

    [Header("Prefabs de Salles")]
    public GameObject[] roomPrefabs; // Tes salles normales
    public GameObject bossRoomPrefab; // LA NOUVELLE VARIABLE : La salle finale

    void Awake()
    {
        // Système Singleton : permet au RoomManager de dire "Génère une salle"
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SpawnNextRoom(Vector3 position)
    {
        // On incrémente le compteur à chaque nouvelle salle demandée
        currentRoomCount++;

        GameObject roomToSpawn;

        // L'Aiguillage
        if (currentRoomCount < maxRooms)
        {
            // Trajet normal : on pioche une salle aléatoire
            int randomIndex = Random.Range(0, roomPrefabs.Length);
            roomToSpawn = roomPrefabs[randomIndex];
        }
        else if (currentRoomCount == maxRooms)
        {
            // Trajet final : on force la salle du boss
            roomToSpawn = bossRoomPrefab;
        }
        else
        {
            // Sécurité absolue : si on dépasse, on arrête tout
            Debug.Log("Le donjon est terminé, on ne génère plus rien !");
            return;
        }

        // L'instanciation de la salle choisie
        Instantiate(roomToSpawn, position, Quaternion.identity);

        // Ta mise à jour de la grille de Pathfinding perso
        PathfindingGrid grid = FindObjectOfType<PathfindingGrid>();
        if (grid != null)
        {
            grid.CreateGrid();
        }
    }
}