using UnityEngine;
using System.Collections;
using TMPro;


public class Dialogue : MonoBehaviour
{
    public string[] dialogueLines;
    private bool isPlayerInTrigger = false;
    private bool isDialogueActive = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<Animator>().SetTrigger("Talk");
            isPlayerInTrigger = true;

            // Inicia el canvas que indica el botón de diálogo
            DialogueManager.Instance.talkCanvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInTrigger = false;
        DialogueManager.Instance.talkCanvas.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.R) && !isDialogueActive)
        {
            // Desactiva el canvas que indica el botón de diálogo
                DialogueManager.Instance.talkCanvas.gameObject.SetActive(false);
                DialogueManager.Instance.StartDialogue(dialogueLines);
                isDialogueActive = true;
        }
    }
}
