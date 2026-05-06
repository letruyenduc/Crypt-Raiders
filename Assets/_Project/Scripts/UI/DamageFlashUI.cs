using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlashUI : MonoBehaviour
{
    public static DamageFlashUI Instance { get; private set; }
    
    public Image flashImage;
    public float flashDuration = 0.2f;
    public Color flashColor = new Color(1, 0, 0, 0.4f);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (flashImage != null) flashImage.color = new Color(0, 0, 0, 0);
    }

    public void TriggerFlash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        flashImage.color = flashColor;
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            Color c = flashColor;
            c.a = Mathf.Lerp(flashColor.a, 0f, elapsed / flashDuration);
            flashImage.color = c;
            yield return null;
        }
        flashImage.color = new Color(0, 0, 0, 0);
    }
}
