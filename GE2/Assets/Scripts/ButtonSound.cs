using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    private static AudioSource audioSource;

    private void Start()
    {
        // Obtener el AudioSource en el primer bot�n que se inicializa
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Este m�todo se llama cuando el mouse entra en el �rea del bot�n
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && audioSource.gameObject.activeInHierarchy)
        {
            // Detener el sonido actual si est� sonando
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Reproducir el sonido
            audioSource.Play();
        }
    }
}
