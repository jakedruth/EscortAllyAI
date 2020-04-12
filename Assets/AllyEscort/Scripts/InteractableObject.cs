using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    /// <summary>
    /// Any function can subscribe to this event
    /// </summary>
    public UnityEvent onInteract;

    /// <summary>
    /// Called to interact with the object
    /// </summary>
    public void Interact()
    {
        onInteract.Invoke();
    }
}
