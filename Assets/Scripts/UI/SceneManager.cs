using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public string nombreEscena; // Nombre de la escena a cargar
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }

    public void Juego()
    {
        Debug.Log("Cargando la escena de juego...");
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nombreEscena, LoadSceneMode.Single);
    }
}
