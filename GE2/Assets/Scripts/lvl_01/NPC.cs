using UnityEngine;
using System;

public class NPC : MonoBehaviour
{
    public Action onInteract; // Evento que se activa cuando el jugador interact�a
    public bool isInQTE = false; // Variable para saber si el NPC est� en un QTE

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
        isInQTE = false; // Marca que ya no est� en QTE
        quickTimeEvent.onQTEEnd -= HandleQTEResult; // Desuscr�bete del evento

        Debug.Log(success ? "Interacci�n completada exitosamente." : "Interacci�n fallida.");
        onInteract?.Invoke(); // Llama al evento de interacci�n para eliminar al NPC
    }
}
