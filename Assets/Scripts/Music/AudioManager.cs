using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    private GameObject audioSource;
    private AudioSource audioSourceComponent;
    public AudioClip[] audioClips;
    public bool isPlaying;
    private void Start()
    {
        audioSource = gameObject;
        audioSourceComponent = audioSource.GetComponentInChildren<AudioSource>(); 
    }
    public void PlayAudio(int index, bool loop)
    {
        if (index < 0 || index >= audioClips.Length)
        {
            Debug.LogWarning("Audio index out of range");
            return;
        }

        audioSourceComponent.clip = audioClips[index];
        audioSourceComponent.loop = loop;
        audioSourceComponent.Play();
        isPlaying = true;
    }

    public void StopAudio()
    {
        audioSourceComponent.Stop();
        isPlaying = false;
    }
}
