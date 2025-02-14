using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public CharacterController Controller;
    public float velocidadNormal = 5f; // Velocidad caminando
    public float velocidadSprint = 10f; // Velocidad corriendo
    public float duracionSprint = 5f; // Duración máxima de sprint
    public float tiempoRecargaSprint = 3f; // Tiempo de recarga después de usar el sprint
    public float gravedad = -9.81f;
    public float salto = 2f;
    public Vector3 direccionjugador;
    public bool tocandopiso;

    public Transform camara; // Cámara del jugador
    public float sensibilidadMouse = 100f; // Sensibilidad del mouse

    private float xRotation = 0f;
    private float tiempoSprintRestante; // Tiempo restante de sprint
    private bool puedeCorrer = true; // Si el personaje puede correr o no
    private float tiempoRecargaRestante = 0f; // Tiempo restante para recargar sprint


    public AudioClip PasosSonido;    // Sonido al recibir sanidad extra
    private AudioSource audioSource; // Audio source del objeto

    void Start()
    {

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.volume = 0.001f;

        // Bloquea el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        tiempoSprintRestante = duracionSprint; // Inicializa el tiempo de sprint
    }

    void Update()
    {
        // Verificar si el personaje está en el suelo
        tocandopiso = Controller.isGrounded;

        // Movimiento del personaje (WASD o flechas)
        Vector3 mover = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveDir = transform.right * mover.x + transform.forward * mover.z;

        // Solo reproducir el sonido si el personaje se está moviendo y está tocando el suelo
        if (moveDir.magnitude > 0 && tocandopiso)
        {
            if (!audioSource.isPlaying && PasosSonido != null)
            {
                audioSource.clip = PasosSonido;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // Manejo del sprint
        if (Input.GetKey(KeyCode.LeftShift) && puedeCorrer && tiempoSprintRestante > 0)
        {
            Controller.Move(moveDir * velocidadSprint * Time.deltaTime);
            tiempoSprintRestante -= Time.deltaTime;
        }
        else
        {
            Controller.Move(moveDir * velocidadNormal * Time.deltaTime);

            if (tiempoSprintRestante <= 0)
            {
                puedeCorrer = false;
                tiempoRecargaRestante -= Time.deltaTime;

                if (tiempoRecargaRestante <= 0)
                {
                    puedeCorrer = true;
                    tiempoSprintRestante = duracionSprint;
                    tiempoRecargaRestante = tiempoRecargaSprint;
                }
            }
        }

        // Movimiento de la cámara con el mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        camara.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Aplicación de gravedad
        if (tocandopiso && direccionjugador.y < 0)
        {
            direccionjugador.y = -2f;
        }

        // Salto
        if (Input.GetButtonDown("Jump") && tocandopiso)
        {
            direccionjugador.y = Mathf.Sqrt(salto * -2f * gravedad);
        }

        direccionjugador.y += gravedad * Time.deltaTime;
        Controller.Move(direccionjugador * Time.deltaTime);
    }


}

