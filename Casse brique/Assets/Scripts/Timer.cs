using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 60; // Temps restant en secondes
    public bool timerIsRunning = false; // Indique si le minuteur est en cours d'exécution
    public TMP_Text timeText; // Référence au texte d'affichage du temps

    void Update()
    {
        // Si le minuteur est en cours d'exécution
        if (timerIsRunning)
        {
            // Si le temps restant est supérieur à zéro
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Réduire le temps restant
                UpdateTimeDisplay(timeRemaining); // Mettre à jour l'affichage du temps
            }
            else
            {
                // Si le temps est écoulé
                timeRemaining = 0;
                timerIsRunning = false; // Arrêter le minuteur
                HandleTimeOut(); // Gérer la fin du temps
            }
        }
    }

    // Mettre à jour l'affichage du temps
    void UpdateTimeDisplay(float timeToDisplay)
    {
        timeToDisplay += 1; // Afficher le temps restant comme une seconde complète
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("Timer: {0:00}:{1:00}", minutes, seconds); // Mettre à jour le texte
    }

    // Démarrer le minuteur
    public void StartTimer()
    {
        timerIsRunning = true; // Marquer le minuteur comme en cours d'exécution
    }

    // Arrêter le minuteur
    public void StopTimer()
    {
        timerIsRunning = false; // Marquer le minuteur comme arrêté
    }

    // Gérer la fin du temps
    void HandleTimeOut()
    {
        Debug.Log("Player loses! Time is up."); // Afficher un message dans la console
        GameController.instance.EndGame(false); // Gérer la fin du jeu lorsque le temps est écoulé
    }
}
