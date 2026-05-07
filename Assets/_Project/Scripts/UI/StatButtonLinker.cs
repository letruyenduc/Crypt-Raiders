using UnityEngine;
using UnityEngine.UI;

public class StatButtonLinker : MonoBehaviour
{
    public enum StatType { HP, Physique, Magie, Reset }
    public StatType type;

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn == null) return;

        // On nettoie les anciens liens morts
        btn.onClick.RemoveAllListeners();

        // On cherche le ProgressionUI de la scène actuelle
        ProgressionUI ui = FindObjectOfType<ProgressionUI>();

        if (ui != null)
        {
            // On rebranche dynamiquement le bouton au script
            switch (type)
            {
                case StatType.HP:
                    btn.onClick.AddListener(ui.OnClickAddHP);
                    break;
                case StatType.Physique:
                    btn.onClick.AddListener(ui.OnClickAddPhys);
                    break;
                case StatType.Magie:
                    btn.onClick.AddListener(ui.OnClickAddMag);
                    break;
                case StatType.Reset:
                    btn.onClick.AddListener(ui.OnClickReset);
                    break;
            }
            Debug.Log($"StatButtonLinker: Bouton {type} rebranché avec succès !");
        }
        else
        {
            Debug.LogError("StatButtonLinker: Impossible de trouver ProgressionUI dans cette scène !");
        }
    }
}
