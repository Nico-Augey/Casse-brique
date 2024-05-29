using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 50f; // Vitesse de déplacement de la barre
    public GameObject ball;   // Référence à la balle
    public float minX = -16.5f; // Limite de déplacement à gauche de la barre
    public float maxX = 16.5f; // Limite de déplacement à droite de la barre
    public TMP_Text startText; // Référence au texte d'initialisation affiché au début
    public Timer gameTimer; // Référence au script Timer

    private bool ballLaunched = false; // Indique si la balle a été lancée
    private bool gameStarted = false; // Indique si le jeu a commencé

    private Vector3 initialBallPosition; // Position initiale de la balle
    private Vector3 initialPlayerPosition; // Position initiale de la barre

    public Rigidbody rb; // Composant Rigidbody de la barre pour gérer la physique

    void Start()
    {
        // Si le texte d'initialisation est assigné, on l'affiche
        if (startText != null)
        {
            startText.gameObject.SetActive(true); // Afficher le texte d'initialisation
        }
        else
        {
            Debug.LogError("StartText is not assigned in the Inspector");
        }

        // Stocker les positions initiales de la balle et de la barre
        initialBallPosition = ball.transform.position;
        initialPlayerPosition = transform.position;
    }

    void Update()
    {
        // Si le jeu n'a pas commencé
        if (!gameStarted)
        {
            // Vérifier si l'utilisateur appuie sur la touche espace pour démarrer le jeu
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameStarted = true; // Marquer le jeu comme commencé
                startText.gameObject.SetActive(false); // Masquer le texte d'initialisation
                StartTimer(); // Démarrer le minuteur
                LaunchBall(); // Lancer la balle
            }
            return; // Sortir de la fonction Update
        }

        // Gérer le déplacement de la barre en fonction des touches A et D
        float move = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            move = -1f; // Déplacer vers la gauche
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move = 1f; // Déplacer vers la droite
        }
        Vector3 newPosition = transform.position + new Vector3(move * speed * Time.deltaTime, 0, 0);
        
        // Limiter le déplacement de la barre aux valeurs minX et maxX
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        transform.position = newPosition;

        // Si aucune touche n'est enfoncée, arrêter le mouvement de la barre
        if (move == 0)
        {
            rb.velocity = Vector3.zero; // Stopper le mouvement du Rigidbody
        }
    }

    // Fonction pour lancer la balle
    public void LaunchBall()
    {
        if (!ballLaunched && ball != null)
        {
            ballLaunched = true; // Marquer la balle comme lancée
            BallController ballController = ball.GetComponent<BallController>();
            if (ballController != null)
            {
                ballController.LaunchBall(); // Appeler la fonction LaunchBall du BallController
            }
            else
            {
                Debug.LogError("BallController component is missing on the ball object.");
            }
        }
        else if (ball == null)
        {
            Debug.LogError("Ball is not assigned in the Inspector");
        }
    }

    // Fonction pour démarrer le minuteur
    public void StartTimer()
    {
        if (gameTimer != null)
        {
            gameTimer.StartTimer(); // Appeler la fonction StartTimer du Timer
        }
        else
        {
            Debug.LogError("Timer is not assigned in the Inspector");
        }
    }

    // Fonction pour réinitialiser le jeu
    public void ResetGame()
    {
        gameStarted = false; // Marquer le jeu comme non commencé
        ballLaunched = false; // Marquer la balle comme non lancée
        ball.transform.position = initialBallPosition; // Réinitialiser la position de la balle
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero; // Réinitialiser la vélocité de la balle
        rb.velocity = Vector3.zero; // Arrêter la vitesse de la barre
        transform.position = initialPlayerPosition; // Réinitialiser la position de la barre
        startText.gameObject.SetActive(true); // Réafficher le texte d'initialisation
        if (gameTimer != null)
        {
            gameTimer.StopTimer(); // Arrêter le minuteur
        }
    }
}
