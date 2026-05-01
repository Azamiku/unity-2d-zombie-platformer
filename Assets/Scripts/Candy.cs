using UnityEngine;

public class Candy : MonoBehaviour
{
    public int points = 15;
    public AudioClip pickupSound; 
    private AudioSource audioSource;

    private void Start()
    {
       
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Sumar puntos
            GameManager.instance.AddScore(points);

            // Reproducir sonido independiente del objeto
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Destruir al instante objeto
            Destroy(gameObject);
        }
    }
}
