using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public string nombreEscena = "JuegoPrincipal";

    public void IniciarCarga()
    {
        StartCoroutine(CargarEscena());
    }

    IEnumerator CargarEscena()
    {
        AsyncOperation carga = SceneManager.LoadSceneAsync(nombreEscena, LoadSceneMode.Additive);

        while (!carga.isDone)
        {
            yield return null;
        }

        // Establecer la nueva escena como activa
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nombreEscena));

        // Descargar la escena actual (menú)
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
