using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance; // Instance du GameController pour le singleton
    public GameObject brickPrefab; // Prefab pour les briques
    public Transform brickContainer; // Conteneur pour les briques
    public int columns = 10; // Nombre de colonnes de briques
    public float brickWidth = 1.0f; // Largeur d'une brique
    public float brickHeight = 0.5f; // Hauteur d'une brique
    public float spacingX = 0.1f; // Espacement horizontal entre les briques
    public float spacingZ = 0.1f; // Espacement vertical entre les briques
    public float waitTime = 0.5f; // Temps d'attente avant réinitialisation
    public TMP_Text endText; // Texte de fin de jeu
    public TMP_Text scoreText; // Texte du score
    public TMP_Text livesText; // Texte des vies
    public PlayerController playerController; // Référence au contrôleur du joueur
    private int score = 0; // Score du joueur
    private int lives = 3; // Vies du joueur

    void Awake()
    {
        // Initialisation du singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Récupérer les dimensions des briques
        Renderer brickRenderer = brickPrefab.GetComponent<Renderer>();
        if (brickRenderer != null)
        {
            brickWidth = brickRenderer.bounds.size.x;
            brickHeight = brickRenderer.bounds.size.z;
        }

        GenerateLevel(); // Générer le niveau
        UpdateScoreText(); // Mettre à jour l'affichage du score
        UpdateLivesText(); // Mettre à jour l'affichage des vies
    }

    // Ajouter des points au score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText(); // Mettre à jour l'affichage du score
        CheckWinCondition(); // Vérifier si le joueur a gagné
    }

    // Mettre à jour le texte du score
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Mettre à jour le texte des vies
    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + lives;
    }

    // Générer le niveau de jeu
    public void GenerateLevel()
    {
        // Supprimer les briques existantes
        foreach (Transform child in brickContainer)
        {
            Destroy(child.gameObject);
        }

        float startX = -columns - 3f / 2f * (brickWidth + spacingX);
        float startZ = -1f;

        // Générer des briques avec des résistances et des positions aléatoires
        for (int col = 0; col < columns; col++)
        {
            int rows = Random.Range(2, 6); // Nombre aléatoire de lignes entre 2 et 5
            for (int row = 0; row < rows; row++)
            {
                Vector3 position = new Vector3(startX + col * (brickWidth + spacingX), 0, startZ - row * (brickHeight + spacingZ));
                GameObject brick = Instantiate(brickPrefab, position, Quaternion.identity, brickContainer);
                brick.GetComponent<Brick>().resistance = Random.Range(1, 5); // Résistance aléatoire entre 1 et 4
                brick.tag = "Brick";
            }
        }
    }

    // Gérer la perte d'une vie
    public void LoseLife()
    {
        lives--;
        UpdateLivesText(); // Mettre à jour l'affichage des vies
        if (lives <= 0)
        {
            EndGame(false); // Fin du jeu si plus de vies
        }
        else
        {
            ResetBallAndPlayer(); // Réinitialiser la balle et le joueur si des vies restent
        }
    }

    // Fin du jeu
    public void EndGame(bool isGameWon)
    {
        SaveDataAndLoadEndScene(isGameWon);
        StartCoroutine(WaitCoroutine(waitTime));
    }

    // Arrêter la balle
    private void StopBall()
    {
        if (playerController.ball != null)
        {
            playerController.ball.GetComponent<Rigidbody>().velocity = Vector3.zero; // Arrêter le mouvement de la balle
        }
    }

    // Sauvegarder les données et charger la scène de fin de jeu
    private void SaveDataAndLoadEndScene(bool isGameWon)
    {
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("IsGameWon", isGameWon ? 1 : 0);
        SceneManager.LoadScene("EndGame"); // Charger la scène de fin de jeu
    }

    // Vérifier si le joueur a gagné
    private void CheckWinCondition()
    {
        if (brickContainer.childCount == 1)
        {
            EndGame(true); // Gagner le jeu si toutes les briques sont détruites
        }
    }

    // Réinitialiser la balle et le joueur
    private void ResetBallAndPlayer()
    {
        StopBall();
        playerController.ResetGame(); // Réinitialiser la balle et le joueur
    }

    // Coroutine pour attendre avant de réinitialiser le jeu
    IEnumerator WaitCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StopBall();
        playerController.ResetGame();
    }
}
