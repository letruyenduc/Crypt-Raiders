using UnityEngine;
using TMPro;
using System.Collections;

public class StartRoomController : MonoBehaviour
{
    public GameObject door;
    public TextMeshProUGUI countdownText;
    public GameObject startButton;
    
    // AJOUTE CETTE LIGNE :
    public Transform nextRoomSpawnPoint; 

    public void OnStartButtonPressed()
    {
        startButton.SetActive(false);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        int timer = 3;
        while (timer > 0)
        {
            countdownText.text = timer.ToString();
            yield return new WaitForSeconds(1f);
            timer--;
        }

        countdownText.text = "GO !";
        OpenDoor();
        
        // AJOUTE CES LIGNES ICI :
        if (DungeonGenerator.instance != null && nextRoomSpawnPoint != null)
        {
            DungeonGenerator.instance.SpawnNextRoom(nextRoomSpawnPoint.position);
        }
        else
        {
            if (DungeonGenerator.instance == null) Debug.LogError("DungeonGenerator.instance est NULL ! Vérifie que le script DungeonGenerator est sur un objet dans la scène.");
            if (nextRoomSpawnPoint == null) Debug.LogError("nextRoomSpawnPoint est NULL ! Glisse l'objet vide (point de sortie) dans l'inspecteur du StartRoomController.");
        }
        
        yield return new WaitForSeconds(1f);
        countdownText.text = "";
    }

    void OpenDoor()
    {
        door.SetActive(false);
        Debug.Log("Le donjon est ouvert !");
    }
}