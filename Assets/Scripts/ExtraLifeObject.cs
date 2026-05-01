using UnityEngine;

public class Heart : MonoBehaviour
{
    [Header("Configuración")]
    public int lifeAmount = 1; // Vidas que da corazón
    public AudioClip pickupSound; // Sonido al recogerlo

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Buscar el componente de salud del jugador
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddLife(lifeAmount);
            }

            // Reproducir sonido si hay uno asignado
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Destruir el corazón
            Destroy(gameObject);
        }
    }
}
