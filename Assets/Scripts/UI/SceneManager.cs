using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public string nombreEscena = "JuegoPrincipal";

    public void IniciarCarga()
    {
        StartCoroutine(CargarEscena());
    }

    IEnumerator CargarEscena()
    {
        AsyncOperation carga = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nombreEscena, LoadSceneMode.Additive);

        while (!carga.isDone)
        {
            yield return null;
        }

        // Establecer la nueva escena como activa
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(nombreEscena));

        // Descargar la escena actual (menú)
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
}
