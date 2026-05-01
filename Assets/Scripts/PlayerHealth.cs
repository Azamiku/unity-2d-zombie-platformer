using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vidas")]
    public int startingLives = 3;   // Vidas al empezar
    public int maxLives = 10;       // Vidas máximas

    private int currentLives;

    [Header("UI")]
    public TextMeshProUGUI livesText; // referencia al texto en pantalla

    private void Start()
    {
        currentLives = startingLives;
        UpdateLivesUI();
    }

    public void TakeDamage(int amount)
    {
        currentLives -= amount;
        if (currentLives < 0) currentLives = 0;

        Debug.Log("Vidas restantes: " + currentLives);
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            // Buscar PlayerRespawn 
            PlayerRespawn respawn = Object.FindFirstObjectByType<PlayerRespawn>();
            if (respawn != null)
                respawn.Respawn();
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // Reiniciar vidas al morir
            currentLives = startingLives;
            UpdateLivesUI();
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Vidas: " + currentLives;
    }

    public void AddLife(int amount)
    {
        currentLives += amount;
        if (currentLives > maxLives)
            currentLives = maxLives;

        UpdateLivesUI();
        Debug.Log("Vidas añadidas: " + amount + " | Vidas actuales: " + currentLives);
    }
}
