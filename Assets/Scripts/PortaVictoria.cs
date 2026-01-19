using UnityEngine;
using TMPro;

public class PortaVictoria : MonoBehaviour
{
    public TextMeshProUGUI textoUI;    
    public GameObject panelVictoria;  
    public AudioSource sonidoVictoria; // Nuevo campo para el audio
    private bool jugadorAProp = false;

    void Start()
    {
        // IMPORTANTE: Al empezar, aseguramos que el jugador no tenga las llaves
        InteraccionLlave.teLaClau = false;
        InteraccionLlave.teLaClauFinal = false;
        Time.timeScale = 1f; // Aseguramos que el tiempo corra si venimos de un reinicio
    }

    void Update()
    {
        if (jugadorAProp)
        {
            // Ahora comprobamos específicamente la LLAVE FINAL
            if (InteraccionLlave.teLaClauFinal)
            {
                textoUI.text = "Prem E per sortir";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Ganar();
                }
            }
            else
            {
                textoUI.text = "Has de trobar la segona clau";
            }
        }
    }

    void Ganar()
    {
       // 1. Reproducir el sonido de victoria
        if (sonidoVictoria != null)
        {
            sonidoVictoria.ignoreListenerPause = true; // Para que suene aunque pausemos el juego
            GameObject.Find("MusicaGlobal").GetComponent<AudioSource>().Stop();
            sonidoVictoria.Play();
        }

        panelVictoria.SetActive(true); 
        textoUI.gameObject.SetActive(false); 
        
        Time.timeScale = 0f; // Congelamos el mundo
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorAProp = true;
            textoUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorAProp = false;
            textoUI.gameObject.SetActive(false);
        }
    }
}