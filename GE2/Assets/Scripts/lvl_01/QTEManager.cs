using UnityEngine;
using UnityEngine.Video;

public class QTEManager : MonoBehaviour
{
    public int requiredQTEs = 5;
    private int successfulQTEs = 0;

    public QuickTimeEvent qtePanel;
    public NPCSpawner npcSpawner; // Referencia al NPCSpawner
    public GameObject panelCinematic; // Panel donde se mostrar� la cinem�tica
    public VideoPlayer videoPlayer; // Referencia al componente VideoPlayer

    void Start()
    {
        if (qtePanel != null)
        {
            qtePanel.onQTEEnd += OnQTECompleted;
        }
        else
        {
            Debug.LogError("QTEManager: No se asign� el script QuickTimeEvent en el panel.");
        }

        if (panelCinematic != null)
        {
            panelCinematic.SetActive(false);
        }
        else
        {
            Debug.LogError("QTEManager: No se asign� el panel de la cinem�tica.");
        }
    }

    void OnQTECompleted(bool success)
    {
        if (success)
        {
            successfulQTEs++;
            Debug.Log("QTE completado con �xito. Total: " + successfulQTEs);

            if (successfulQTEs >= requiredQTEs)
            {
                TriggerCinematic();
            }
        }
    }

    void TriggerCinematic()
    {
        Debug.Log("Cinem�tica activada.");
        npcSpawner?.StopSpawning(); // Detener el spawner
        panelCinematic?.SetActive(true); // Activar el panel
        // Congela el tiempo en el juego
        Time.timeScale = 0f;

        if (videoPlayer != null)
        {
            videoPlayer.Play(); // Reproducir el video
        }
        else
        {
            Debug.LogError("No se asign� un VideoPlayer.");
        }
    }
}
