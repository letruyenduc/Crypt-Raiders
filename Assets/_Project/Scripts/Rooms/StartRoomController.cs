using UnityEngine;
using TMPro; // Si tu utilises TextMeshPro
using System.Collections;

public class StartRoomController : MonoBehaviour
{
    public GameObject door;           // Glisse ta porte ici
    public TextMeshProUGUI countdownText; // Glisse ton texte UI ici
    public GameObject startButton;    // Glisse ton bouton UI ici

    public void OnStartButtonPressed()
    {
        startButton.SetActive(false); // Cache le bouton
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
        
        yield return new WaitForSeconds(1f);
        countdownText.text = ""; // Efface le texte
    }

    void OpenDoor()
    {
        door.SetActive(false); // Ouvre physiquement la porte
        Debug.Log("Le donjon est ouvert !");
    }
}