using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Loads win scene when the player enters the trigger
        if (other.CompareTag("Player"))
        {
            // Load the win scene
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("WinScene", LoadSceneMode.Additive);
        }
    }
}
