using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPulse : MonoBehaviour
{
    public float pulseSpeed = 1f; // Vitesse de la pulsation du texte
    private TMP_Text text; // Composant TMP_Text pour gérer le texte

    void Start()
    {
        // Récupérer le composant TMP_Text attaché à l'objet
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        // Calculer l'échelle de pulsation en fonction du temps
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * 0.05f;
        // Appliquer la nouvelle échelle au texte
        text.transform.localScale = new Vector3(scale, scale, scale);
    }
}
