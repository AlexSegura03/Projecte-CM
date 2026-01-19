using UnityEngine;
using TMPro;
using System.Collections;

public class Porta : MonoBehaviour
{
    public TextMeshProUGUI textoUI;
    
    [Header("Configuració Temporal")]
    public MeshRenderer visualPorta;  
    public BoxCollider coliderSolid;  
    public float tempsOberta = 4f;    

    private bool jugadorAProp = false;
    private bool estaOberta = false;
    private bool portaBloquejada = false; // Nueva variable para el bloqueo final

    void Start()
    {
        InteraccionLlave.teLaClau = false;
    }

    // Función "puente" para evitar el error CS1061 del FirstPersonController
    public void ObrirPorta()
    {
        if (!estaOberta && !portaBloquejada) 
        {
            StartCoroutine(ObrirTemporbalment());
        }
    }

    void Update()
    {
        // Si la puerta ya se bloqueó, no hacemos nada más
        if (portaBloquejada) return;

        if (jugadorAProp && !estaOberta)
        {
            if (InteraccionLlave.teLaClau) 
            {
                textoUI.text = "Prem E per passar ";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ObrirPorta();
                }
            }
            else
            {
                textoUI.text = "Has de trobar la clau";
            }
        }
    }

    IEnumerator ObrirTemporbalment()
    {
        estaOberta = true;
        textoUI.gameObject.SetActive(false);

        // 1. Abrimos la puerta
        visualPorta.enabled = false;
        coliderSolid.enabled = false;
        Debug.Log("Porta oberta... corre!");

        // 2. Esperamos el tiempo de seguridad
        yield return new WaitForSeconds(tempsOberta);

        // 3. Cerramos la puerta para siempre
        visualPorta.enabled = true;
        coliderSolid.enabled = true;
        estaOberta = false;
        portaBloquejada = true; // <--- AQUÍ se bloquea para siempre
        
        Debug.Log("La puerta se ha sellado. Ya no puedes volver.");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Solo mostramos el texto si la puerta NO está bloqueada
        if (other.CompareTag("Player") && !portaBloquejada)
        {
            jugadorAProp = true;
            if (!estaOberta) textoUI.gameObject.SetActive(true);
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