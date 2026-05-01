using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ScoreMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoresText;

    private void Start()
    {
        ShowScores();
    }

    private void ShowScores()
    {
        // Lista de jugadores guardados (ejemplo simple de 3 jugadores)
        List<(string name, int score)> players = new List<(string, int)>();

        // Obtenemos datos guardados en PlayerPrefs
        for (int i = 1; i <= 3; i++)
        {
            string nameKey = "Player" + i + "_Name";
            string scoreKey = "Player" + i + "_Score";

            if (PlayerPrefs.HasKey(nameKey) && PlayerPrefs.HasKey(scoreKey))
            {
                string name = PlayerPrefs.GetString(nameKey);
                int score = PlayerPrefs.GetInt(scoreKey);
                players.Add((name, score));
            }
        }

        // Agregamos al jugador actual si lo quieres mostrar
        string lastName = PlayerPrefs.GetString("LastPlayer", "Jugador");
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        players.Add((lastName, lastScore));

        // Ordenar de mayor a menor
        players = players.OrderByDescending(p => p.score).ToList();

        // Construir el texto
        string display = "";
        int rank = 1;
        foreach (var p in players)
        {
            display += $"{rank}. {p.name}: {p.score}\n";
            rank++;
        }

        if (scoresText != null)
            scoresText.text = display;
    }
}
