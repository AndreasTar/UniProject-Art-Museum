using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorInteract : MonoBehaviour
{
    public string interactMessage = "Press 'E' to open the door";
    private bool canInteract = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player has a "Player" tag
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            // Put code here to open the door
            Debug.Log("Door opened!"); // Example: just logging the opening of the door
        }
    }

    private void OnGUI()
    {
        if (canInteract)
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 200, 50), interactMessage);
        }
    }
}
