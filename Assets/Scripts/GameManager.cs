using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    private TextMeshProUGUI scoreText;

    [Header("Puntuación")]
    public int currentScore = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // se ejecuta cada vez que cambia la escena
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Busca el objeto de texto de score automáticamente
        GameObject scoreObj = GameObject.FindWithTag("ScoreText");
        if (scoreObj != null)
        {
            scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
            UpdateScoreUI();
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
        SaveScore();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + currentScore;
    }

    private void SaveScore()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Jugador");
        PlayerPrefs.SetInt("LastScore", currentScore);
        PlayerPrefs.SetString("LastPlayer", playerName);
        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
        SaveScore();
    }

    public void SaveToLeaderboard()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Jugador");
        int playerScore = currentScore;

        // Guardamos el nuevo jugador y puntuación en una lista de texto separada por comas
        string existingData = PlayerPrefs.GetString("Leaderboard", "");
        string newEntry = playerName + ":" + playerScore;

        // Si ya hay datos, los añadimos con un separador
        if (!string.IsNullOrEmpty(existingData))
            existingData += "," + newEntry;
        else
            existingData = newEntry;

        PlayerPrefs.SetString("Leaderboard", existingData);
        PlayerPrefs.Save();

        Debug.Log($"Guardado en Leaderboard: {newEntry}");
    }
}