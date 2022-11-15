using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Add or remove unity events to the game object
    public bool useEvents;
    // Message to display to players
    public string prompyMessage;
    public void BaseInteract(GameObject interactble = null)
    {
        if (useEvents)
            GetComponent<InteractionEvents>().OnInteract.Invoke(interactble);
        Interact(interactble);
    }
    protected virtual void Interact(GameObject interactable = null)
    {
        // Overwrite the function
    }
}
