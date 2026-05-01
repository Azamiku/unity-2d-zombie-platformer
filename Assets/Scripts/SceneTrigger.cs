using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public SceneManagement sceneManagement;
    public bool goNext = true;
    public Transform spawnPointOnNextScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Guardar checkpoint
        if (PlayerData.instance != null && spawnPointOnNextScene != null)
        {
            PlayerData.instance.lastCheckpointPosition = spawnPointOnNextScene.position;
            Debug.Log("Checkpoint guardado: " + PlayerData.instance.lastCheckpointPosition);
        }

        // Cambiar escena
        if (sceneManagement != null)
        {
            if (goNext)
                sceneManagement.GoToNextScene();
            else
                sceneManagement.GoToPreviousScene();
        }
    }
}
