using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; // Added to access UI elements like Image

public class BeatManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Intervals[] _intervals;
    public float BPM = 160f; // BPM for the intervals
    public UnityEngine.UI.Image beatBar; // UI element to show the beat

    private void Update()
    {
        foreach (Intervals interval in _intervals)
        {
            // Para cambiar el BPM, sólo cambiar el parámetro que le pasamos a GetIntervalLength, es decir, BPMSelection.BPMSelectionInstance.BPM
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(BPM)));
            interval.CheckForNewInterval(sampledTime);
        }

        beatBar.fillAmount = 1f - Mathf.PingPong(Time.time / (60f / BPM), 1f); // Invert the beat bar fill amount
        
        if (beatBar.fillAmount <= 0f)
        {
            beatBar.fillAmount = 1f; // Reset the fill amount when it reaches 0
        }
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float _steps;
    [SerializeField] private UnityEvent _trigger;
    private int _lastInterval;

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * _steps);
    }

    public void CheckForNewInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
}