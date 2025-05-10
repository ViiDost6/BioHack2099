using UnityEngine;

public class ExitOnPlay : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //check if escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //exit game
            Application.Quit();
            Debug.Log("Saliendo del juego...");
        }
    }
}
