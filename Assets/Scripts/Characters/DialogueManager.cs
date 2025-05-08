using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public Canvas talkCanvas;
    public Canvas dialogueCanvas;
    public Canvas playerUICanvas;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private Coroutine dialogueRoutine;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dialogueCanvas.gameObject.SetActive(false);
        talkCanvas.gameObject.SetActive(false);
        playerUICanvas.gameObject.SetActive(true);
    }

    public void StartDialogue(string[] lines)
    {
        if (dialogueRoutine != null)
            StopCoroutine(dialogueRoutine);

        dialogueRoutine = StartCoroutine(ShowDialogue(lines));
    }

    IEnumerator ShowDialogue(string[] lines)
    {
        playerUICanvas.gameObject.SetActive(false);
        dialogueCanvas.gameObject.SetActive(true);
        foreach (string line in lines)
        {
            dialogueText.text = "";
            foreach (char letter in line)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            // Esperar a que el jugador presione R
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
        }

        dialogueCanvas.gameObject.SetActive(false);
        playerUICanvas.gameObject.SetActive(true);
        dialogueRoutine = null;
    }
}

