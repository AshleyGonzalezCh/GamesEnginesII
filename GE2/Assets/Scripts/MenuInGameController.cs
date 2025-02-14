using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInGameController : MonoBehaviour
{
    public GameObject gameOverPanel; //Espacio para arrastrar el panel
    public GameObject winPanel; //Espacio para arrastrar el panel
    public GameObject UIPanel; //Espacio para arrastrar el panel
    public GameObject menuPanel; //Espacio para arrastrar el panel
    public string Escena;
    public string Escena2;

    public void Salir()
    {
        Application.Quit();
    }
    void Start()
    {
        // Desactivar todos los paneles que no son necesarios al iniciar el juego
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        menuPanel.SetActive(false);
        //Desactiva y bloquea el cursor para mantenerse en la pantalla del juego
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TriggerMenu();
        }
    }


    //---------------------------------------
    //---------------------------------------

    public void TriggerGameOver()
    {
        // Activa el panel de Game Over
        gameOverPanel.SetActive(true);
        UIPanel.SetActive(false);
        winPanel.SetActive(false);
        //Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // Congela el tiempo en el juego
        Time.timeScale = 0f;
    }

    public void TriggerWin()
    {
        // Activa el panel de Win
        gameOverPanel.SetActive(false);
        UIPanel.SetActive(false);
        winPanel.SetActive(true);
        //Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // Congela el tiempo en el juego
        Time.timeScale = 0f;
    }

    public void TriggerMenu()
    {
        // Activa el panel de menu
        gameOverPanel.SetActive(false);
        UIPanel.SetActive(false);
        winPanel.SetActive(false);
        menuPanel.SetActive(true);
        //Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // Congela el tiempo en el juego
        Time.timeScale = 0f;
    }

    public void VolverJuego()
    {
        // Desactiva el menu y vuelve a la pantalla del juego
        gameOverPanel.SetActive(false);
        UIPanel.SetActive(true);
        winPanel.SetActive(false);
        menuPanel.SetActive(false);
        //Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Devuelve el tiempo en el juego
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Carga la escena actual para reiniciar el juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverPanel.SetActive(false);
        menuPanel.SetActive(false);
        winPanel.SetActive(false);
        UIPanel.SetActive(true);
        // Devuelve el tiempo en el juego
        Time.timeScale = 1f;
    }

    public void CargarEscena()
    {
        SceneManager.LoadScene(Escena, LoadSceneMode.Single);
    }

    public void CargarEscena2()
    {
        SceneManager.LoadScene(Escena2, LoadSceneMode.Single);
    }

}
