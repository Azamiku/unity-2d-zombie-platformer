using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text leaderboardText; 

    private const string PlayerNameKey = "PlayerName";
    private const string LastScoreKey = "LastScore";
    private const string LastCheckpointXKey = "LastCheckpointX";
    private const string LastCheckpointYKey = "LastCheckpointY";
    private const string ScoreCountKey = "ScoreCount";

    private void Start()
    {
        ShowLeaderboard();
    }

    // --- NUEVA PARTIDA ---
    public void StartNewGame()
    {
        string playerName = inputField != null ? inputField.text : "";
        if (string.IsNullOrEmpty(playerName))
            playerName = "Jugador";

        PlayerPrefs.SetString(PlayerNameKey, playerName);
        PlayerPrefs.SetInt(LastScoreKey, 0);
        PlayerPrefs.SetFloat(LastCheckpointXKey, 0f);
        PlayerPrefs.SetFloat(LastCheckpointYKey, 0f);
        PlayerPrefs.Save();

        if (GameManager.instance != null)
            GameManager.instance.ResetScore();

        SceneManager.LoadScene("Scene_Start");
    }

    // --- CONTINUAR PARTIDA ---
    public void ContinueGame()
    {
        string playerName = PlayerPrefs.GetString(PlayerNameKey, "");
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("No hay partida guardada. Inicia una nueva.");
            return;
        }

        SceneManager.LoadScene("Scene_Start");
    }

    // --- GUARDAR PROGRESO ---
    public static void SaveProgress(Vector3 checkpoint, int score)
    {
        PlayerPrefs.SetFloat(LastCheckpointXKey, checkpoint.x);
        PlayerPrefs.SetFloat(LastCheckpointYKey, checkpoint.y);
        PlayerPrefs.SetInt(LastScoreKey, score);
        PlayerPrefs.Save();
        Debug.Log("Progreso guardado: Pos({checkpoint.x}, {checkpoint.y}), Score: {score}");
    }

    // --- CARGAR PROGRESO ---
    public static void LoadProgress(out Vector3 checkpoint, out int score)
    {
        float x = PlayerPrefs.GetFloat(LastCheckpointXKey, 0f);
        float y = PlayerPrefs.GetFloat(LastCheckpointYKey, 0f);
        checkpoint = new Vector3(x, y, 0f);
        score = PlayerPrefs.GetInt(LastScoreKey, 0);
        Debug.Log("Progreso cargado: Pos({x}, {y}), Score: {score}");
    }

    // --- GUARDAR SCORE FINAL EN EL RANKING ---
    public static void SaveScore(string playerName, int score)
    {
        int count = PlayerPrefs.GetInt(ScoreCountKey, 0);
        PlayerPrefs.SetString("ScoreName_{count}", playerName);
        PlayerPrefs.SetInt("ScoreValue_{count}", score);
        PlayerPrefs.SetInt(ScoreCountKey, count + 1);
        PlayerPrefs.Save();
    }

    // --- MOSTRAR RANKING EN EL MENÚ ---
    private void ShowLeaderboard()
    {
        if (leaderboardText == null) return;

        int count = PlayerPrefs.GetInt(ScoreCountKey, 0);
        List<(string name, int score)> scores = new();

        for (int i = 0; i < count; i++)
        {
            string name = PlayerPrefs.GetString("ScoreName_{i}", "Jugador");
            int score = PlayerPrefs.GetInt("ScoreValue_{i}", 0);
            scores.Add((name, score));
        }

        scores = scores.OrderByDescending(s => s.score).ToList();

        leaderboardText.text = " Puntuaciones:\n";
        for (int i = 0; i < scores.Count; i++)
        {
            leaderboardText.text += "{i + 1}. {scores[i].name} — {scores[i].score}\n";
        }
    }
}
