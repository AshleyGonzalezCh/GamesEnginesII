using UnityEngine;
using System;

public class NPC : MonoBehaviour
{
    public Action onInteract; // Evento que se activa cuando el jugador interactúa
    public bool isInQTE = false; // Variable para saber si el NPC está en un QTE

    private QuickTimeEvent quickTimeEvent; // Referencia al script de QuickTimeEvent

    private void Start()
    {
        quickTimeEvent = FindObjectOfType<QuickTimeEvent>();
    }

    public void StartQTE()
    {
        if (!isInQTE && quickTimeEvent != null)
        {
            quickTimeEvent.StartNewQTE();
            isInQTE = true;

            quickTimeEvent.onQTEEnd += HandleQTEResult;
        }
    }

    private void HandleQTEResult(bool success)
    {
        isInQTE = false; // Marca que ya no está en QTE
        quickTimeEvent.onQTEEnd -= HandleQTEResult; // Desuscríbete del evento

        Debug.Log(success ? "Interacción completada exitosamente." : "Interacción fallida.");
        onInteract?.Invoke(); // Llama al evento de interacción para eliminar al NPC
    }
}
