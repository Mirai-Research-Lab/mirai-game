using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Add or remove unity events to the game object
    public bool useEvents;
    // Message to display to players
    public string prompyMessage;
    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvents>().OnInteract.Invoke();
        Interact();
    }
    protected virtual void Interact()
    {
        // Overwrite the function
    }
}
