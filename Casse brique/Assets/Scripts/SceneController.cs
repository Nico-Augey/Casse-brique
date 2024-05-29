using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Charger la scène principale du jeu
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1); // Charger la scène avec l'index 1
    }

    // Quitter l'application du jeu
    public void ExitButton()
    {
        Application.Quit(); // Fermer l'application
        Debug.Log("Game closed"); // Afficher un message dans la console
    }

    // Charger la scène du menu principal
    public void Menu()
    {
        SceneManager.LoadSceneAsync(0); // Charger la scène avec l'index 0
    }

    // Charger la scène du tableau des scores
    public void ScoreBoard()
    {
        SceneManager.LoadSceneAsync(3); // Charger la scène avec l'index 3
    }
}
