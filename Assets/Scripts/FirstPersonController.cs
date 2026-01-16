using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour
{
    [Header("Moviment")]
    public CharacterController controller;
    public float velocitatMoviment = 12f;
    public float velocitatSalt = 3f;
    public float gravetat = -9.81f;

    [Header("Córrer")]
    public KeyCode teclaCorrer = KeyCode.LeftControl;
    public float multiplicadorCorrer = 1.6f;

    [Header("Terra")]
    public Transform groundCheck;
    public float distanciaTerra = 0.4f;
    public LayerMask mascaraTerra;

    [Header("Vista")]
    public float sensibilitatRatoli = 200f;
    public Camera cameraPOV;

    [Header("Interacció")]
    public float distanciaInteraccio = 3f;
    public LayerMask capaInteraccio; // Recorda assignar això al Inspector!
    public KeyCode teclaInteractuar = KeyCode.E;
    public bool teClau = false; // Inventari simple

    [Header("Llanterna")]
    public Transform llanternaPivot;
    public Light llanternaLight;
    public KeyCode teclaLlanterna = KeyCode.F;

    [Header("Efectes Llanterna")]
    public float retardRotacio = 8f;
    public float intensitatBase = 3f;
    public float flickerVelocitat = 15f;
    public float flickerIntensitat = 0.3f;
    public float tremolorRotacio = 0.5f;
    public float balanceigMoviment = 1.5f;

    [Header("Fallades Llanterna")]
    public float tempsMinEntreFallades = 6f;
    public float tempsMaxEntreFallades = 15f;
    public float duracioApagada = 1.2f;
    public int parpelleigsMin = 2;
    public int parpelleigsMax = 3;

    [Header("So Llanterna")]
    public AudioSource audioSource;
    public AudioClip soClickManual;
    public AudioClip soClickFallada;

    Vector3 velocitatVertical;
    bool estaAlTerra;
    float rotacioVertical;
    bool llanternaActiva = true;
    bool falladaActiva = false;

    float temporitzadorFallada;
    Quaternion rotacioObjectiu;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraPOV == null)
            cameraPOV = Camera.main;

        if (llanternaLight != null)
            intensitatBase = llanternaLight.intensity;
            
        ResetTemporitzadorFallada();
    }

    void Update()
    {
        ComprovarTerra();
        Moviment();
        SaltIGravetat();
        Vista();
        Llanterna();
        LogicaInteraccio(); // NOVA FUNCIÓ
    }

    void ComprovarTerra()
    {
        estaAlTerra = Physics.CheckSphere(groundCheck.position, distanciaTerra, mascaraTerra);
        if (estaAlTerra && velocitatVertical.y < 0)
            velocitatVertical.y = -2f;
    }

    void Moviment()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool corrent = Input.GetKey(teclaCorrer) && z > 0.1f;

        float velocitatActual = corrent
            ? velocitatMoviment * multiplicadorCorrer
            : velocitatMoviment;

        Vector3 moviment = transform.right * x + transform.forward * z;
        controller.Move(moviment * velocitatActual * Time.deltaTime);
    }

    void SaltIGravetat()
    {
        if (Input.GetButtonDown("Jump") && estaAlTerra)
            velocitatVertical.y = Mathf.Sqrt(velocitatSalt * -2f * gravetat);

        velocitatVertical.y += gravetat * Time.deltaTime;
        controller.Move(velocitatVertical * Time.deltaTime);
    }

    void Vista()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilitatRatoli * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilitatRatoli * Time.deltaTime;

        rotacioVertical -= mouseY;
        rotacioVertical = Mathf.Clamp(rotacioVertical, -90f, 90f);

        cameraPOV.transform.localRotation = Quaternion.Euler(rotacioVertical, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void LogicaInteraccio()
    {
        if (Input.GetKeyDown(teclaInteractuar))
        {
            RaycastHit hit;
            // Llancem un raig des del centre de la càmera
            if (Physics.Raycast(cameraPOV.transform.position, cameraPOV.transform.forward, out hit, distanciaInteraccio, capaInteraccio))
            {
                // CAS 1: Trobem la clau
                if (hit.collider.CompareTag("Clau"))
                {
                    teClau = true;
                    Debug.Log("Has agafat la clau!");
                    
                    if (audioSource != null && soClickManual != null) 
                        audioSource.PlayOneShot(soClickManual); // So feedback

                    Destroy(hit.collider.gameObject);
                }
                // CAS 2: Trobem la porta
                else if (hit.collider.CompareTag("Porta"))
                {
                    // Busquem el script "Porta" dins l'objecte
                    Porta scriptPorta = hit.collider.GetComponent<Porta>();

                    if (scriptPorta != null)
                    {
                        if (teClau)
                        {
                            Debug.Log("Obrint porta...");
                            scriptPorta.ObrirPorta();
                        }
                        else
                        {
                            Debug.Log("Està tancada. Necessites la clau.");
                            if (audioSource != null && soClickFallada != null) 
                                audioSource.PlayOneShot(soClickFallada); // So de tancat
                        }
                    }
                }
            }
        }
    }

    void Llanterna()
    {
        if (llanternaPivot == null || llanternaLight == null)
            return;

        // Toggle manual
        if (Input.GetKeyDown(teclaLlanterna))
        {
            llanternaActiva = !llanternaActiva;
            llanternaLight.enabled = llanternaActiva;

            PlayClick(false); // manual

            if (llanternaActiva)
                ResetTemporitzadorFallada();
        }

        if (!llanternaActiva || falladaActiva)
            return;

        // --- FALLADA DINÀMICA ---
        float moviment = controller.velocity.magnitude;
        float multiplicador = moviment > velocitatMoviment * 0.6f ? 0.4f : 1f;

        temporitzadorFallada -= Time.deltaTime * (1f / multiplicador);

        if (temporitzadorFallada <= 0f)
        {
            StartCoroutine(FalladaLlanterna());
            ResetTemporitzadorFallada();
            return;
        }

        // Rotació objectiu
        rotacioObjectiu = Quaternion.Euler(
            cameraPOV.transform.eulerAngles.x,
            transform.eulerAngles.y,
            0f
        );

        Quaternion tremolor = Quaternion.Euler(
            Random.Range(-tremolorRotacio, tremolorRotacio),
            Random.Range(-tremolorRotacio, tremolorRotacio),
            0f
        );

        llanternaPivot.rotation = Quaternion.Slerp(
            llanternaPivot.rotation,
            rotacioObjectiu * tremolor,
            retardRotacio * Time.deltaTime
        );

        float balanceig = Mathf.Sin(Time.time * 6f) * moviment * balanceigMoviment;
        llanternaPivot.localRotation *= Quaternion.Euler(balanceig, 0f, 0f);

        float flicker = Mathf.PerlinNoise(Time.time * flickerVelocitat, 0f);
        llanternaLight.intensity = intensitatBase + (flicker - 0.5f) * flickerIntensitat;
    }

    IEnumerator FalladaLlanterna()
    {
        falladaActiva = true;

        int parpelleigs = Random.Range(parpelleigsMin, parpelleigsMax + 1);

        for (int i = 0; i < parpelleigs; i++)
        {
            llanternaLight.enabled = false;
            PlayClick(true); // fallada
            yield return new WaitForSeconds(Random.Range(0.05f, 0.12f));
            llanternaLight.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.12f));
        }

        llanternaLight.enabled = false;
        PlayClick(true);

        llanternaActiva = false; 
        falladaActiva = false;
    }

    void ResetTemporitzadorFallada()
    {
        temporitzadorFallada = Random.Range(tempsMinEntreFallades, tempsMaxEntreFallades);
    }

    void PlayClick(bool esFallada)
    {
        if (audioSource == null) return;

        audioSource.pitch = esFallada
            ? Random.Range(0.85f, 1.1f)
            : 1f;

        AudioClip clip = esFallada ? soClickFallada : soClickManual;

        if (clip != null)
            audioSource.PlayOneShot(clip);

        audioSource.pitch = 1f;
    }
}