using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Scene_Menu"); 
    }
}
