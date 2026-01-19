using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class ControladorMenu : MonoBehaviour
{
    public void EmpezarJuego()
    {
        // Ponemos el nombre exacto de tu escena de juego
        SceneManager.LoadScene("SampleScene"); 
    }

    public void SalirDelJuego()
    {
        Debug.Log("Surtint...");
        Application.Quit();
    }
}