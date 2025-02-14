using TMPro;
using UnityEngine;

public class Interacciones : MonoBehaviour
{
    public TMP_Text interactText;
    private NPC currentNPC = null;
    private bool isNearNPC = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isNearNPC && other.CompareTag("NPC"))
        {
            Debug.Log("Jugador entró al rango de interacción del NPC.");
            isNearNPC = true;
            currentNPC = other.GetComponent<NPC>();
            interactText.gameObject.SetActive(true); // Mostrar texto de interacción
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isNearNPC && other.CompareTag("NPC"))
        {
            Debug.Log("Jugador salió del rango de interacción del NPC.");
            isNearNPC = false;
            currentNPC = null;
            interactText.gameObject.SetActive(false); // Ocultar texto de interacción
        }
    }


    private void Update()
    {
        // Activar el QTE solo si se presiona la tecla E
        if (isNearNPC && Input.GetKeyDown(KeyCode.E) && currentNPC != null)
        {
            interactText.gameObject.SetActive(false); // Ocultar texto de interacción
            currentNPC.StartQTE(); // Inicia el QTE del NPC
        }
    }
}
