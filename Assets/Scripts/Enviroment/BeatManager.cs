using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Intervals[] _intervals;
    public float BPM = 160f; // BPM for the intervals

    private void Update()
    {
        foreach (Intervals interval in _intervals)
        {
            // Para cambiar el BPM, sólo cambiar el parámetro que le pasamos a GetIntervalLength, es decir, BPMSelection.BPMSelectionInstance.BPM
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(BPM)));
            interval.CheckForNewInterval(sampledTime);
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