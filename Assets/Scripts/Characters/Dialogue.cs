using UnityEngine;
using System.Collections;
using TMPro;


public class Dialogue : MonoBehaviour
{
    
    public string[] dialogueLines; // Array to hold the dialogue lines
    public float typingSpeed = 0.05f; // Speed of typing effect
    public Canvas talkCanvas; // Reference to the canvas for displaying action
    public Canvas dialogueBox ; // Reference to the canvas for displaying dialogue
    public GameObject door; // Reference to the door object if needed
    private bool isDialogueActive = false; // Flag to check if dialogue is active
    public bool isDoorLocked; // Flag to check if the door must be locked until the player finishes the dialogue

    private int currentLineIndex = 0; // Index of the current line being displayed

    void Start()
    {
        talkCanvas.gameObject.SetActive(false); // Hide the canvas at the start
        dialogueBox.gameObject.SetActive(false); // Hide the dialogue box at the start
        currentLineIndex = 0; // Initialize the line index
        isDialogueActive = false; // Initialize the dialogue active flag
        
        if (isDoorLocked)
        {
            door.GetComponent<Doors>().canDoorBeOpened = false; // Lock the door if needed
        }
    }

    void DialogueManager()
    {
        if (Input.GetKeyDown(KeyCode.R) && isDialogueActive == false)
        {
            isDialogueActive = true; // Set the dialogue active flag to true
            currentLineIndex = 0; // Reset the line index to start from the first line

            // Start the coroutine to show the dialogue
            StartCoroutine(ShowDialogue(currentLineIndex));
        }
        else if (Input.GetKeyDown(KeyCode.R) && isDialogueActive == true)
        {
            //stops last coroutine if the player presses "R" again
            StopAllCoroutines(); // Stop any active dialogue coroutine

            // If dialogue is active and "R" is pressed, show the next line
            currentLineIndex++;
            if (currentLineIndex < dialogueLines.Length)
            {
                StartCoroutine(ShowDialogue(currentLineIndex)); // Show the next line
            }
            else
            {
                currentLineIndex = 0; // Reset index if all lines have been shown
                isDialogueActive = false; // Set the dialogue active flag to false
                dialogueBox.gameObject.SetActive(false); // Hide the dialogue box after showing all lines

                if (isDoorLocked)
                {
                    door.GetComponent<Doors>().canDoorBeOpened = true; // Unlock the door if needed
                }
            }
        }
    }

    IEnumerator ShowDialogue(int lineIndex)
    {
        // Show the dialogue box and set the text to the current line
        dialogueBox.gameObject.SetActive(true);
        TextMeshProUGUI textComponent = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = ""; // Clear previous text

        // Type out the current line
        foreach (char letter in dialogueLines[lineIndex].ToCharArray())
        {
            textComponent.text += letter; // Add each letter to the text component
            yield return new WaitForSeconds(typingSpeed); // Wait for the specified typing speed
        }

        while (!Input.GetKeyDown(KeyCode.R))
        {
            // Wait until the player presses the "R" key to continue
            Debug.Log("Press R to continue"); // Debug message for testing
            yield return null;
        }

        currentLineIndex++; // Move to the next line

        if (currentLineIndex >= dialogueLines.Length)
        {
            currentLineIndex = 0; // Reset index if all lines have been shown
            isDialogueActive = false; // Set the dialogue active flag to false
            dialogueBox.gameObject.SetActive(false); // Hide the dialogue box after showing all lines
        }
    }

    

    void Update()
    {
        //checks if the dialogue array is empty
        if (dialogueLines.Length >= 0)
        {
            DialogueManager();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //cuando el jugador entra en el trigger, se activa la animacion de saludar
        if (other.CompareTag("Player"))
            this.GetComponent<Animator>().SetTrigger("Talk");
            //show canvas indicating that the player can talk to the NPC
            talkCanvas.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        // When the player exits the trigger
        if (other.CompareTag("Player"))
        {
            talkCanvas.gameObject.SetActive(false); // Hide the canvas when player exits the trigger
            dialogueBox.gameObject.SetActive(false); // Hide the dialogue box when player exits the trigger
            currentLineIndex = 0; // Reset index for next interaction

            // Stop any active dialogue coroutine
            StopAllCoroutines();
        }
    }
}
