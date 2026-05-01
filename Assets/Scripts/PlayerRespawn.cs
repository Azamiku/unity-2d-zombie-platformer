using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public void Respawn()
    {
        if (PlayerData.instance != null)
        {
            Vector3 pos = PlayerData.instance.lastCheckpointPosition;
            if (pos != Vector3.zero)
            {
                transform.position = pos;
                Debug.Log("Jugador reaparece en checkpoint: " + pos);
            }
            else
            {
                transform.position = Vector3.zero;
                Debug.Log("No hay checkpoint guardado. Reaparece en (0,0,0).");
            }
        }
        else
        {
            Debug.LogError("No existe PlayerData.instance al intentar reaparecer.");
        }

        // Reinicia velocidad por seguridad
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }
}
