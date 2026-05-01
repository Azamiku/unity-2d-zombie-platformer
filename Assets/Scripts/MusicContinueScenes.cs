using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance; 

    private void Awake()
    {
        // Si ya hay una instancia, se destruye la nueva
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Orden de No destruir al cambiar de escena
    }
}
