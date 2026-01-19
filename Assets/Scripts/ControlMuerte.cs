using UnityEngine;

public class ControlMuerte : MonoBehaviour
{
    public GameObject pantallaDerrota; // El PanelMuerte que ya tienes
    public AudioSource sonidoMuerte;   // Arrastra aquí el nuevo AudioSource

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Morir();
        }
    }

    void Morir()
    {
        if (sonidoMuerte != null)
        {
            // 1. Esto hace que el sonido ignore si el juego se pausa
            sonidoMuerte.ignoreListenerPause = true; 
            sonidoMuerte.Play();
        }

        pantallaDerrota.SetActive(true);
        
        // El juego se congela AQUÍ, pero el sonido seguirá sonando
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}