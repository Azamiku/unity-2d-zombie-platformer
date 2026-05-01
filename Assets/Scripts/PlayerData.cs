using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public Vector3 lastCheckpointPosition;
    public int lastScore = 0;

    private const string LastCheckpointXKey = "LastCheckpointX";
    private const string LastCheckpointYKey = "LastCheckpointY";
    private const string LastScoreKey = "LastScore";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Cargamos datos automáticamente al iniciar
        LoadGame();
    }

    // Guardar la partida en PlayerPrefs
    public void SaveGame()
    {
        PlayerPrefs.SetFloat(LastCheckpointXKey, lastCheckpointPosition.x);
        PlayerPrefs.SetFloat(LastCheckpointYKey, lastCheckpointPosition.y);
        PlayerPrefs.SetInt(LastScoreKey, lastScore);
        PlayerPrefs.Save();

        Debug.Log($" Partida guardada: posición {lastCheckpointPosition}, score {lastScore}");
    }

    // Cargar la partida de PlayerPrefs
    public void LoadGame()
    {
        float x = PlayerPrefs.GetFloat(LastCheckpointXKey, 0f);
        float y = PlayerPrefs.GetFloat(LastCheckpointYKey, 0f);
        lastCheckpointPosition = new Vector3(x, y, 0f);
        lastScore = PlayerPrefs.GetInt(LastScoreKey, 0);

        Debug.Log($" Partida cargada: posición {lastCheckpointPosition}, score {lastScore}");
    }

    // Opción para resetear progreso (al iniciar nueva partida)
    public void ResetData()
    {
        lastCheckpointPosition = Vector3.zero;
        lastScore = 0;
        SaveGame();
    }
}
