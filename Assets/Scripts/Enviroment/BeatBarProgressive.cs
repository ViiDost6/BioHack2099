using UnityEngine;
using UnityEngine.UI;

public class BeatBarProgressive : MonoBehaviour
{
    [Header("UI")]
    public Image fillImage; // La imagen con 'Image Type: Filled'

    [Header("Beat Timing")]
    public float intervalDuration = 1; // Tiempo entre beats, en segundos

    private float timer = 0f;
    private bool counting = false;

    /// <summary>
    /// Llamado desde el evento del beat.
    /// Reinicia el tiempo y comienza el llenado.
    /// </summary>
    public void OnBeat()
    {
        timer = 0f;
        counting = true;
    }

    /// <summary>
    /// Opcional: Puedes detener el contador si lo necesitas.
    /// </summary>
    public void StopProgress()
    {
        counting = false;
        fillImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (!counting || intervalDuration <= 0f || fillImage == null) return;

        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / intervalDuration);
        fillImage.fillAmount = progress;
    }

    /// <summary>
    /// Llamar esto si cambias el BPM dinámicamente.
    /// </summary>
    public void SetIntervalFromBPM(float bpm, float steps = 1f)
    {
        if (bpm <= 0 || steps <= 0) return;
        intervalDuration = 60f / (bpm * steps);
    }
}
