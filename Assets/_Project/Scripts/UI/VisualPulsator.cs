using UnityEngine;

public class VisualPulsator : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.1f;

    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        transform.localScale = baseScale * scale;
    }
}
