using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class ScoreboardController : MonoBehaviour
{
    public RectTransform contentTransform; // Référence au transform du contenu de ScrollRect
    public GameObject textTemplate; // Template pour les éléments de texte
    public string csvFilePath = "highscores.csv"; // Chemin vers le fichier CSV des scores

    void Start()
    {
        LoadHighScores(); // Charger les scores élevés
        textTemplate.gameObject.SetActive(false); // Désactiver le template après chargement
    }

    // Charger les scores élevés depuis le fichier CSV
    void LoadHighScores()
    {
        string path = Path.Combine(Application.persistentDataPath, csvFilePath);

        if (!File.Exists(path))
        {
            Debug.LogWarning("No high scores file found."); // Avertir si aucun fichier de scores n'est trouvé
            return;
        }

        string[] lines = File.ReadAllLines(path); // Lire toutes les lignes du fichier
        List<string> sortedScores = new List<string>();

        // Parcourir chaque ligne et vérifier le format
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 2 && int.TryParse(parts[1], out int score))
            {
                sortedScores.Add(line); // Ajouter la ligne si le format est correct
            }
            else
            {
                Debug.LogWarning("Invalid line format: " + line); // Avertir si le format est incorrect
            }
        }

        // Trier les scores par ordre décroissant
        sortedScores.Sort((a, b) => int.Parse(b.Split(',')[1]).CompareTo(int.Parse(a.Split(',')[1])));

        // Créer et afficher les éléments de texte pour chaque score
        for (int i = 0; i < sortedScores.Count; i++)
        {
            string line = sortedScores[i];
            string[] parts = line.Split(',');
            string playerName = parts[0];
            string score = parts[1];

            GameObject newText = Instantiate(textTemplate, contentTransform);
            newText.GetComponent<TMP_Text>().text = $"{i + 1}. {playerName}     :    {score}";
            
            // Ajuster la position sur l'axe Y
            RectTransform rectTransform = newText.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -i * 35f);
            }
            
            newText.SetActive(true); // Assurez-vous que le template est actif après l'instanciation
        }
    }
}
