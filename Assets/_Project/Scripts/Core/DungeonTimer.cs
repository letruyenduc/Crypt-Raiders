using UnityEngine;
using TMPro;
using System.Collections;

public class DungeonTimer : MonoBehaviour
{
    public static DungeonTimer Instance { get; private set; }

    public float timeRemaining = 300f; // 5 minutes
    private bool timerIsRunning = false;

    [Header("UI")]
    public GameObject timerContainer; 
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI penaltyText; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        // On cache tout au début
        if (timerContainer != null) timerContainer.SetActive(false);
        if (penaltyText != null) penaltyText.gameObject.SetActive(false);
    }

    public void StartTimer()
    {
        // On affiche le chrono et on lance la machine
        if (timerContainer != null) timerContainer.SetActive(true);
        timerIsRunning = true;
        Debug.Log("DungeonTimer: Chrono lancé !");
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                if (GameOverManager.instance != null) GameOverManager.instance.TriggerGameOver();
            }
        }
    }

    public void DeductTime(float seconds)
    {
        timeRemaining -= seconds;
        if (timeRemaining < 0) timeRemaining = 0;
        
        StopCoroutine("ShowPenaltyEffect");
        StartCoroutine(ShowPenaltyEffect(seconds));
    }

    private IEnumerator ShowPenaltyEffect(float seconds)
    {
        if (penaltyText == null) yield break;

        penaltyText.text = $"-{seconds}s";
        penaltyText.gameObject.SetActive(true);
        timerText.color = Color.red;
        
        yield return new WaitForSeconds(2.0f);
        
        penaltyText.gameObject.SetActive(false);
        timerText.color = Color.white;
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
