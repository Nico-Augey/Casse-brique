using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int resistance = 1; // Résistance de la brique
    private Renderer rend; // Composant Renderer pour afficher la brique
    public Material material1; // Matériau pour résistance 1
    public Material material2; // Matériau pour résistance 2
    public Material material3; // Matériau pour résistance 3
    public Material material4; // Matériau pour résistance 4

    void Start()
    {
        rend = GetComponent<Renderer>(); // Récupérer le composant Renderer
        UpdateColor(); // Mettre à jour la couleur de la brique en fonction de la résistance
    }

    // Fonction appelée lorsque la brique est touchée
    public void Hit()
    {
        resistance--; // Réduire la résistance de la brique
        GameController.instance.AddScore(1); // Ajouter des points pour avoir touché une brique
        if (resistance <= 0)
        {
            Destroy(gameObject); // Détruire la brique si la résistance est 0
            GameController.instance.AddScore(3); // Ajouter des points supplémentaires pour avoir détruit la brique
        }
        else
        {
            UpdateColor(); // Mettre à jour la couleur en fonction de la résistance restante
        }
    }

    // Fonction pour mettre à jour la couleur de la brique
    void UpdateColor()
    {
        Material materialToUse = null;
        switch (resistance)
        {
            case 4: materialToUse = material4; break;
            case 3: materialToUse = material3; break;
            case 2: materialToUse = material2; break;
            case 1: materialToUse = material1; break;
        }

        if (materialToUse != null)
        {
            rend.material = materialToUse; // Définir le matériau de la brique
        }
        else
        {
            Debug.LogWarning("No material assigned for the given resistance value.");
        }
    }
}
