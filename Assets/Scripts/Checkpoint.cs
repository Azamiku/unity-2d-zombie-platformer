using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Guardar posición en PlayerData
        PlayerData.instance.lastCheckpointPosition = transform.position;

        // Guardar progreso en PlayerPrefs
        PlayerPrefs.SetFloat("LastCheckpointX", transform.position.x);
        PlayerPrefs.SetFloat("LastCheckpointY", transform.position.y);

        // Guardar puntuación actual
        if (GameManager.instance != null)
            PlayerPrefs.SetInt("LastScore", GameManager.instance.currentScore);

        PlayerPrefs.Save();

        Debug.Log("Checkpoint alcanzado: " + transform.position +
                  " | Score: " + (GameManager.instance != null ? GameManager.instance.currentScore : 0));
    }
}
