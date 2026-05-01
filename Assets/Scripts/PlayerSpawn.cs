using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Start()
    {
        if (PlayerData.instance == null)
        {
            Debug.LogError("PlayerData.instance es null!");
            return;
        }

        // Cargar los datos guardados
        PlayerData.instance.LoadGame();

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No se encontró GameObject con tag 'Player'");
            return;
        }

        // Colocar al jugador en el checkpoint guardado
        if (PlayerData.instance.lastCheckpointPosition != Vector3.zero)
            player.transform.position = PlayerData.instance.lastCheckpointPosition;
        else
            player.transform.position = transform.position;

        Debug.Log("PlayerSpawn mueve al jugador a: " + player.transform.position);

        // restaurar la puntuación:
        GameManager.instance.currentScore = PlayerData.instance.lastScore;
    }
}
