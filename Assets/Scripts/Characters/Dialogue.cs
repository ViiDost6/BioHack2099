using UnityEngine;
using System.Collections;


public class Dialogue : MonoBehaviour
{
    
    public string[] dialogueLines; // Array to hold the dialogue lines
    public float typingSpeed = 0.05f; // Speed of typing effect
    public Canvas talkCanvas; // Reference to the canvas for displaying action
    public Canvas dialogueBox ; // Reference to the canvas for displaying dialogue
    public GameObject door; // Reference to the door object if needed

    private int currentLineIndex = 0; // Index of the current line being displayed

    void Start()
    {
        talkCanvas.gameObject.SetActive(false); // Hide the canvas at the start
        dialogueBox.gameObject.SetActive(false); // Hide the dialogue box at the start
        currentLineIndex = 0; // Initialize the line index
    }

    void DialogueManager()
    {
        // If the player presses R, the dialogue box is activated and the first line of dialogue is displayed
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Stop any existing dialogue coroutine to prevent overlapping
            StopAllCoroutines();

            dialogueBox.gameObject.SetActive(true);
            talkCanvas.gameObject.SetActive(false);

            // Calls the coroutine to display the dialogue
            DisplayDialogue(dialogueLines[currentLineIndex]);
        }
    }

    void DisplayDialogue(string line)
    {
        // Clear the previous text
        dialogueBox.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ""; // Clear the text in the dialogue box

        // Type out the current line of dialogue
        foreach (char letter in line.ToCharArray())
        {
            dialogueBox.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += letter; // Add each letter to the text box
            yield return new WaitForSeconds(typingSpeed); // Wait for the specified typing speed before adding the next letter
        }

        // Wait for player input to proceed to the next line or close the dialogue box
        while (!Input.GetKeyDown(KeyCode.R))
        {
            yield return null; // Wait until the player presses R again
        }

        currentLineIndex++; // Move to the next line

        // Checks for more lines of dialogue
        if (currentLineIndex < dialogueLines.Length)
        {
            // If there are more lines, call the coroutine again to display the next line
            DisplayDialogue(dialogueLines[currentLineIndex]);
        }
        else if (currentLineIndex >= dialogueLines.Length)
        {
            // If no more lines, hide the dialogue box and reset the index
            dialogueBox.gameObject.SetActive(false); // Hide the dialogue box when finished
            currentLineIndex = 0; // Reset index for next interaction
        }
    }
    // Update is called once per frame


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
