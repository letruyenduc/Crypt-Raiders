using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float moveSpeed = 2f;
    public float lifetime = 1.0f;
    private Color textColor;

    private void Awake()
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(int damage)
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshPro>();
        if (textMesh == null) return;

        textMesh.text = damage.ToString();
        textColor = textMesh.color;
        
        // Un peu d'aléatoire pour le mouvement
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 1f, 0f).normalized;
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = randomDir * moveSpeed;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (textMesh == null) return;

        // Fade out progressif
        textColor.a -= (1.0f / lifetime) * Time.deltaTime;
        textMesh.color = textColor;
    }
}
