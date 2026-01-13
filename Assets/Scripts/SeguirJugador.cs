using UnityEngine;
using UnityEngine.AI; // Esto es obligatorio para la IA

public class SeguirJugador : MonoBehaviour
{
    public Transform objetivo; // Aquí irá tu objeto "Jugador"
    private NavMeshAgent agente;

    void Start()
    {
        // Buscamos el componente NavMeshAgent que pusimos antes
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (objetivo != null)
        {
            // Le ordenamos al agente ir a la posición del jugador
            agente.destination = objetivo.position;
        }
    }
}