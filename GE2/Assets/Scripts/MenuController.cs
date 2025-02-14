using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string Escena; // Nombre de la escena a cargar
    public GameObject panelCreditos; // Referencia al panel de créditos
    public GameObject panelMenu; // Referencia al panel de créditos

    public void CargarEscena()
    {
        SceneManager.LoadScene(Escena, LoadSceneMode.Single);
        Debug.LogWarning("INICIAR");
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void MostrarCreditos()
    {
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(true); // Activa el panel de créditos
            panelMenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("El panel de créditos no está asignado.");
        }
        Debug.LogWarning("CREDITOS");
    }

    public void VolverAlMenu()
    {
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false); // Desactiva el panel de créditos
            panelMenu.SetActive(true);
        }
        else
        {
            Debug.LogWarning("El panel de créditos no está asignado.");
        }
    }
}

