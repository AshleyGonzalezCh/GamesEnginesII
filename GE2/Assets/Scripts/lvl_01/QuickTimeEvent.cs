using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickTimeEvent : MonoBehaviour
{
    public Slider progressBar;
    public Slider skillCheckBar;
    public Image safeZoneImage;
    public TMP_Text resultText;

    public float minProgressSpeed = 0.1f;
    public float maxProgressSpeed = 0.2f;
    public float minSkillCheckSpeed = 0.3f;
    public float maxSkillCheckSpeed = 0.7f;
    public float safeZoneMinSize = 0.1f;
    public float safeZoneMaxSize = 0.3f;

    private float progressSpeed;
    private float skillCheckSpeed;
    private bool isQTEActive;
    private int maxAttempts = 3;
    private int attemptsLeft;
    private bool movingRight = true;

    private float safeZoneStart;
    private float safeZoneEnd;

    public AudioClip ErrorSound; // Sonido que se reproducirá al spawnear un NPC
    private AudioSource audioSource;

    public Action<bool> onQTEEnd;

    void Start()
    {
        // Asegúrate de que haya un componente AudioSource adjunto
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en el GameObject del NPCSpawner.");
        }
        ResetQTEState();
    }

    void Update()
    {
        if (isQTEActive)
        {
            if (movingRight)
            {
                skillCheckBar.value += skillCheckSpeed * Time.deltaTime;
                if (skillCheckBar.value >= 1f) movingRight = false;
            }
            else
            {
                skillCheckBar.value -= skillCheckSpeed * Time.deltaTime;
                if (skillCheckBar.value <= 0f) movingRight = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                AttemptSkillCheck();
            }
        }
    }

    public void StartNewQTE()
    {
        ResetQTEState();
        isQTEActive = true;
        attemptsLeft = maxAttempts;

        progressSpeed = UnityEngine.Random.Range(minProgressSpeed, maxProgressSpeed);
        skillCheckSpeed = UnityEngine.Random.Range(minSkillCheckSpeed, maxSkillCheckSpeed);

        progressBar.gameObject.SetActive(true);
        skillCheckBar.gameObject.SetActive(true);
        safeZoneImage.gameObject.SetActive(true);
        resultText.gameObject.SetActive(false);

        progressBar.value = 0f;
        skillCheckBar.value = 0f;

        SetupSafeZone();
    }

    void SetupSafeZone()
    {
        float safeZoneSize = UnityEngine.Random.Range(safeZoneMinSize, safeZoneMaxSize);
        safeZoneStart = UnityEngine.Random.Range(0f, 1f - safeZoneSize);
        safeZoneEnd = safeZoneStart + safeZoneSize;

        RectTransform safeZoneRect = safeZoneImage.rectTransform;
        safeZoneRect.anchorMin = new Vector2(safeZoneStart, 0f);
        safeZoneRect.anchorMax = new Vector2(safeZoneEnd, 1f);
        safeZoneRect.offsetMin = Vector2.zero;
        safeZoneRect.offsetMax = Vector2.zero;
    }

    public void AttemptSkillCheck()
    {
        if (!isQTEActive) return;

        float checkPosition = skillCheckBar.value;

        if (checkPosition >= safeZoneStart && checkPosition <= safeZoneEnd)
        {
            progressBar.value += 0.2f;
            if (progressBar.value >= 1f)
            {
                EndQTE(true);
            }
            else
            {
                SetupSafeZone(); // Si acierta, actualizamos la zona segura para el siguiente intento
            }
        }
        else
        {
            // Detener cualquier sonido en reproducción antes de reproducir el error
            if (audioSource.isPlaying)
            {
                audioSource.Stop();  // Detener el sonido actual si está reproduciéndose
            }

            // Reproducir sonido de error
            if (ErrorSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(ErrorSound);
            }
            else
            {
                if (ErrorSound == null)
                    Debug.LogWarning("El AudioClip 'ErrorSound' no está asignado.");
                if (audioSource == null)
                    Debug.LogWarning("El AudioSource no está configurado correctamente.");
            }

            attemptsLeft--;
            if (attemptsLeft <= 0)
            {
                EndQTE(false);
            }
            else
            {
                ResetSkillCheck(); // Si falla, reinicia la barra de habilidad con una nueva zona segura
            }
        }

    }

    void ResetSkillCheck()
    {
        skillCheckBar.value = 0f; // Reseteamos la posición de la barra de habilidad
        movingRight = true; // Aseguramos que comience moviéndose hacia la derecha
        SetupSafeZone(); // Creamos una nueva zona segura
    }

    void EndQTE(bool success)
    {
        isQTEActive = false;

        progressBar.gameObject.SetActive(false);
        skillCheckBar.gameObject.SetActive(false);
        safeZoneImage.gameObject.SetActive(false);

        resultText.gameObject.SetActive(true);
        resultText.text = success ? "¡Éxito!" : "Fallaste";

        StartCoroutine(HideResultText()); // Oculta el texto después de unos segundos

        onQTEEnd?.Invoke(success);
    }

    void ResetQTEState()
    {
        isQTEActive = false;
        progressBar.gameObject.SetActive(false);
        skillCheckBar.gameObject.SetActive(false);
        safeZoneImage.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
    }

    IEnumerator HideResultText()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos antes de ocultar el texto
        resultText.gameObject.SetActive(false);
    }
}
