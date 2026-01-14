using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class MenuDerrota : MonoBehaviour
{
    public void ReiniciarPartida()
    {
        // Volvemos a poner el tiempo normal antes de cargar
        Time.timeScale = 1f; 
        
        // Cargamos la escena que está abierta actualmente
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}