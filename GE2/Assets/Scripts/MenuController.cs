using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject panelCreditos;
    public GameObject panelMenu;
    public GameObject panelSeleccionNivel;

    public AudioSource clickSound; // Sonido al hacer clic en un botón

    private void Start()
    {
        panelSeleccionNivel.SetActive(false);
        panelCreditos.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void CargarEscena(string escena)
    {
        ReproducirSonidoClick();
        SceneManager.LoadScene(escena, LoadSceneMode.Single);
    }

    public void Salir()
    {
        ReproducirSonidoClick();
        Application.Quit();
    }

    public void MostrarCreditos()
    {
        ReproducirSonidoClick();
        panelCreditos.SetActive(true);
        panelMenu.SetActive(false);
    }

    public void VolverAlMenu()
    {
        ReproducirSonidoClick();
        panelCreditos.SetActive(false);
        panelSeleccionNivel.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void MostrarSeleccionNivel()
    {
        ReproducirSonidoClick();
        panelMenu.SetActive(false);
        panelSeleccionNivel.SetActive(true);
    }


    public void ReproducirSonidoClick()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
    }
}
