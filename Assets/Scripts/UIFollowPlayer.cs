using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    public Transform player;                 // Emy
    public Vector3 offset = new Vector3(1.5f, 5, 0); // Distancia sobre  Emy

    private void LateUpdate()
    {
        if (player != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(player.position + offset);
        }
    }
}
