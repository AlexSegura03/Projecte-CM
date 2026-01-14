using UnityEngine;

public partial class ControlMuerte : MonoBehaviour
{
    public GameObject pantallaDerrota; // Arrastra aquí tu texto de "Has perdido"

    private void OnTriggerEnter(Collider other)
    {
        // Si lo que toca al monstruo tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            Derrota();
        }
    }

    void Derrota()
    {
        pantallaDerrota.SetActive(true); // Mostramos el texto
        Time.timeScale = 0f;            // Congelamos el juego
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}