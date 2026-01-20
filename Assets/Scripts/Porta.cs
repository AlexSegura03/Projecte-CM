using UnityEngine;
using TMPro;
using System.Collections;

public class Porta : MonoBehaviour
{
    public TextMeshProUGUI textoUI;

    [Header("Configuració Porta")]
    public float tempsOberta = 4f;
    public float angleObert = 90f;
    public float velocitatGir = 2f;

    [Header("Frontissa")]
    public Vector3 offsetFrontissa = new Vector3(-0.25f, 0f, 0f); // posició de la frontissa respecte al pivot
    public bool obrirCapEndins = true;


    [Header("Collider Porta")]
    public Vector3 midaCollider = new Vector3(0.5f, 10f, 10f);
    public Vector3 offsetCollider = new Vector3(0f, 5f, 0f);

    [Header("Temps Espera")]
    public float tempsEsperaClau = 0.5f; // Segons que espera entre so clau i gir


    [Header("So de la porta")]
    public AudioClip clipClau;
    public AudioClip clipRovellat;
    public AudioSource audioSource; // només un AudioSource que reprodueix tots dos clips


    private bool jugadorAProp = false;
    private bool estaOberta = false;
    private bool portaBloquejada = false;

    private Quaternion rotacioTancada;
    private Quaternion rotacioOberta;

    private BoxCollider colliderSolid;
    private BoxCollider colliderTrigger;
    private Vector3 puntFrontissaWorld;


    void Start()
    {
        InteraccionLlave.teLaClau = false;

        // Guardem punt de frontissa en coordenades WORLD
        puntFrontissaWorld = transform.position + transform.TransformDirection(offsetFrontissa);

        rotacioTancada = transform.rotation;
        rotacioOberta = Quaternion.Euler(transform.eulerAngles + new Vector3(0, angleObert, 0));

        colliderSolid = gameObject.AddComponent<BoxCollider>();
        colliderSolid.isTrigger = false;
        colliderSolid.size = midaCollider;
        colliderSolid.center = offsetCollider;

        colliderTrigger = gameObject.AddComponent<BoxCollider>();
        colliderTrigger.isTrigger = true;
        colliderTrigger.size = midaCollider + new Vector3(1f, 0f, 1f);
        colliderTrigger.center = offsetCollider;
    }


    public void ObrirPorta()
    {
        if (!estaOberta && !portaBloquejada)
        {
            StartCoroutine(ObrirTemporalment());
        }
    }

    void Update()
    {
        if (portaBloquejada) return;

        if (jugadorAProp && !estaOberta)
        {
            if (InteraccionLlave.teLaClau)
            {
                textoUI.text = "Prem E per passar";
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

    IEnumerator ObrirTemporalment()
    {
        estaOberta = true;
        textoUI.gameObject.SetActive(false);

        // 1️⃣ Reproduir so de la clau
        if (clipClau != null)
            audioSource.PlayOneShot(clipClau, 3f);

        // 2️⃣ Esperar 0.5 segon abans de girar
        yield return new WaitForSeconds(tempsEsperaClau);

        // 3️⃣ Reproduir so rovellat de la porta
        if (clipRovellat != null)
            audioSource.PlayOneShot(clipRovellat, 6f);

        // 4️⃣ Girar porta
        yield return StartCoroutine(GirarPorta(angleObert));

        colliderSolid.enabled = false;
        Debug.Log("Porta oberta... corre!");

        // 5️⃣ Esperar tempsOberta abans de tancar
        yield return new WaitForSeconds(tempsOberta);

        // 6️⃣ Girar porta per tancar
        yield return StartCoroutine(GirarPorta(-angleObert));

        colliderSolid.enabled = true;
        estaOberta = false;
        portaBloquejada = true;

        Debug.Log("La porta s'ha segellat per sempre.");
    }

    IEnumerator GirarPorta(float angleFinal)
    {
        float angleActual = 0f;

        // IMPORTANT: direcció depèn de si obres o tanques
        float direccio = angleFinal > 0 ? 1f : -1f;
        if (!obrirCapEndins) direccio *= -1f;

        while (angleActual < Mathf.Abs(angleFinal))
        {
            float pas = Time.deltaTime * velocitatGir * 90f;
            if (angleActual + pas > Mathf.Abs(angleFinal))
                pas = Mathf.Abs(angleFinal) - angleActual;

            transform.RotateAround(
                puntFrontissaWorld,
                Vector3.up,
                pas * direccio
            );

            angleActual += pas;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
