using UnityEngine;

public class FixedUI : MonoBehaviour
{
    private Transform target; 
    private Vector3 offset;   

    void Start()
    {
        // On récupère le parent actuel comme cible à suivre
        target = transform.parent;
        
        if (target != null)
        {
            // On calcule le décalage (offset) par rapport à la position mondiale
            offset = transform.position - target.position;
            
            // On se détache du parent ! 
            // Désormais, les flips, rotations et animations de l'ennemi ne nous affectent plus.
            transform.SetParent(null);
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            // Si l'ennemi meurt et disparaît, on supprime la barre
            Destroy(gameObject);
            return;
        }

        // On suit la position de l'ennemi avec le décalage initial
        transform.position = target.position + offset;

        // On force la rotation et le scale à rester neutres
        transform.rotation = Quaternion.identity;
        // On ne touche pas au scale ici pour garder celui défini dans l'inspecteur au départ
    }
}
