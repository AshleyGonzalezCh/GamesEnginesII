using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    private static AudioSource audioSource;

    private void Start()
    {
        // Obtener el AudioSource en el primer botón que se inicializa
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Este método se llama cuando el mouse entra en el área del botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && audioSource.gameObject.activeInHierarchy)
        {
            // Detener el sonido actual si está sonando
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Reproducir el sonido
            audioSource.Play();
        }
    }
}
