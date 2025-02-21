using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; 
    public CanvasGroup fadeCanvas;  
    public string cargarEscena = "lvl_01"; 

    void Start()
    {
        Time.timeScale = 0f;
        fadeCanvas.alpha = 1f;
        fadeCanvas.blocksRaycasts = true;

        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.Play();
        // Espera hasta que el video realmente empiece a reproducirse
        while (!videoPlayer.isPlaying)
        {
            yield return null;
        }

        Debug.Log("Video en reproducción.");

        // Esperar hasta que el video termine
        yield return new WaitForSecondsRealtime((float)videoPlayer.length);

        Debug.Log("Video finalizado, iniciando fade.");
        StartCoroutine(FadeOutAndStartGame());
    }

    IEnumerator FadeOutAndStartGame()
    {
        float fadeDuration = 1.5f;
        float time = 0f;

        // Comienza el fade
        while (time < fadeDuration)
        {
            fadeCanvas.alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeCanvas.alpha = 0;
        fadeCanvas.blocksRaycasts = false;


        // Espera hasta que el jugador presione una tecla
        yield return new WaitUntil(() => Input.anyKeyDown);

        // Cargar la primera escena (nivel 1)
        SceneManager.LoadScene(cargarEscena, LoadSceneMode.Single);
    }
}
