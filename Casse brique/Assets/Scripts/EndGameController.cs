using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class EndGameController : MonoBehaviour
{
    public TMP_Text finalScoreText; // Texte du score final
    public TMP_Text gameResultText; // Texte du résultat de la partie
    public TMP_InputField playerNameInput; // Champ de saisie du nom du joueur
    public string csvFilePath = "highscores.csv"; // Chemin vers le fichier CSV des scores

    void Start()
    {
        // Récupérer le score final et le résultat de la partie depuis les préférences du joueur
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        bool isGameWon = PlayerPrefs.GetInt("IsGameWon", 0) == 1;

        // Mettre à jour le texte du score final
        finalScoreText.text = "Final Score: " + finalScore;
        if (isGameWon)
        {
            gameResultText.text = "You Won!";
            gameResultText.color = Color.green; // Changer la couleur en vert si gagné
        }
        else
        {
            gameResultText.text = "Game Over";
            gameResultText.color = Color.red; // Changer la couleur en rouge si perdu
        }
    }

    // Enregistrer le score élevé
    public void SaveHighScore()
    {
        string playerName = playerNameInput.text;
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player name is empty. Please enter a name."); // Avertir si le nom du joueur est vide
            return;
        }

        // Vérifier que le texte du score final contient bien le score sous forme de texte
        string scoreText = finalScoreText.text;
        if (string.IsNullOrEmpty(scoreText))
        {
            Debug.LogWarning("Score is empty. Please make sure the score is set."); // Avertir si le score est vide
            return;
        }

        // Extraire le score de la chaîne de texte
        string[] parts = scoreText.Split(':');
        if (parts.Length != 2 || !int.TryParse(parts[1].Trim(), out int score))
        {
            Debug.LogWarning("Failed to parse the score. Please ensure the score format is correct."); // Avertir si l'extraction du score échoue
            return;
        }

        // Enregistrer le score dans le fichier CSV
        string newScoreEntry = playerName + "," + score;
        string path = Path.Combine(Application.persistentDataPath, csvFilePath);

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(newScoreEntry); // Écrire la nouvelle entrée de score dans le fichier
        }
    }
}
