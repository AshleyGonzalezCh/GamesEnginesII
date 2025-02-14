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
            Debug.Log("Jugador entr� al rango de interacci�n del NPC.");
            isNearNPC = true;
            currentNPC = other.GetComponent<NPC>();
            interactText.gameObject.SetActive(true); // Mostrar texto de interacci�n
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isNearNPC && other.CompareTag("NPC"))
        {
            Debug.Log("Jugador sali� del rango de interacci�n del NPC.");
            isNearNPC = false;
            currentNPC = null;
            interactText.gameObject.SetActive(false); // Ocultar texto de interacci�n
        }
    }


    private void Update()
    {
        // Activar el QTE solo si se presiona la tecla E
        if (isNearNPC && Input.GetKeyDown(KeyCode.E) && currentNPC != null)
        {
            interactText.gameObject.SetActive(false); // Ocultar texto de interacci�n
            currentNPC.StartQTE(); // Inicia el QTE del NPC
        }
    }
}
