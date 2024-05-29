using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 50f; // Vitesse de la balle
    private Rigidbody rb; // Composant Rigidbody de la balle
    private Vector3 direction; // Direction de la balle
    private bool isSpeedBoosted = false; // Indique si la balle est en vitesse boostée

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Récupérer le composant Rigidbody
        rb.velocity = Vector3.zero; // Initialiser la vélocité de la balle à zéro
    }

    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * speed; // Maintenir une vitesse constante
    }

    // Fonction pour lancer la balle
    public void LaunchBall()
    {
        // Déterminer un angle aléatoire dans un cône de ±10 degrés
        float angle = Random.Range(-10f, 10f);
        float angleRad = angle * Mathf.Deg2Rad; // Convertir l'angle en radians

        // Calculer la direction en fonction de l'angle
        direction = new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad));

        // Appliquer la vélocité à la balle
        rb.velocity = direction * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            collision.gameObject.GetComponent<Brick>().Hit(); // Gérer la collision avec une brique
        }
        else if (collision.gameObject.CompareTag("murBas"))
        {
            GameController.instance.LoseLife(); // Gérer la collision avec le mur du bas
        }
        else if (collision.gameObject.CompareTag("RedBrick"))
        {
            StartCoroutine(SpeedBoost()); // Activer le boost de vitesse
        }

        // Refléter la vélocité de la balle après une collision
        Vector3 normal = collision.contacts[0].normal;
        Vector3 reflectDir = Vector3.Reflect(rb.velocity, normal);
        rb.velocity = reflectDir.normalized * speed;
    }

    // Coroutine pour gérer le boost de vitesse
    IEnumerator SpeedBoost()
    {
        if (!isSpeedBoosted)
        {
            isSpeedBoosted = true;
            speed *= 2; // Doubler la vitesse de la balle
            yield return new WaitForSeconds(15); // Attendre 15 secondes
            speed /= 2; // Réduire la vitesse à la normale
            isSpeedBoosted = false;
        }
    }
}
